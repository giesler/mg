namespace System.Data.Services.Client
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Reflection;
    using System.Text;

    internal class PathBox
    {
        private readonly Dictionary<ParameterExpression, string> basePaths = new Dictionary<ParameterExpression, string>(ReferenceEqualityComparer<ParameterExpression>.Instance);
        private const char EntireEntityMarker = '*';
        private readonly List<StringBuilder> expandPaths = new List<StringBuilder>();
        private readonly Stack<ParameterExpression> parameterExpressions = new Stack<ParameterExpression>();
        private readonly List<StringBuilder> projectionPaths = new List<StringBuilder>();

        internal PathBox()
        {
            this.projectionPaths.Add(new StringBuilder());
        }

        private static void AddEntireEntityMarker(StringBuilder sb)
        {
            if (sb.Length > 0)
            {
                sb.Append('/');
            }
            sb.Append('*');
        }

        internal void AppendToPath(PropertyInfo pi)
        {
            StringBuilder builder;
            Debug.Assert(pi != null, "pi != null");
            Type elementType = TypeSystem.GetElementType(pi.PropertyType);
            if (ClientType.CheckElementTypeIsEntity(elementType))
            {
                builder = this.expandPaths.Last<StringBuilder>();
                Debug.Assert(builder != null);
                if (builder.Length > 0)
                {
                    builder.Append('/');
                }
                builder.Append(pi.Name);
            }
            builder = this.projectionPaths.Last<StringBuilder>();
            Debug.Assert(builder != null, "sb != null -- we are always building paths in the context of a parameter");
            RemoveEntireEntityMarkerIfPresent(builder);
            if (builder.Length > 0)
            {
                builder.Append('/');
            }
            builder.Append(pi.Name);
            if (ClientType.CheckElementTypeIsEntity(elementType))
            {
                AddEntireEntityMarker(builder);
            }
        }

        internal void PopParamExpression()
        {
            this.parameterExpressions.Pop();
        }

        internal void PushParamExpression(ParameterExpression pe)
        {
            StringBuilder item = this.projectionPaths.Last<StringBuilder>();
            this.basePaths.Add(pe, item.ToString());
            this.projectionPaths.Remove(item);
            this.parameterExpressions.Push(pe);
        }

        private static void RemoveEntireEntityMarkerIfPresent(StringBuilder sb)
        {
            if ((sb.Length > 0) && (sb[sb.Length - 1] == '*'))
            {
                sb.Remove(sb.Length - 1, 1);
            }
            if ((sb.Length > 0) && (sb[sb.Length - 1] == '/'))
            {
                sb.Remove(sb.Length - 1, 1);
            }
        }

        internal void StartNewPath()
        {
            Debug.Assert(this.ParamExpressionInScope != null, "this.ParamExpressionInScope != null -- should not be starting new path with no lambda parameter in scope.");
            StringBuilder sb = new StringBuilder(this.basePaths[this.ParamExpressionInScope]);
            RemoveEntireEntityMarkerIfPresent(sb);
            this.expandPaths.Add(new StringBuilder(sb.ToString()));
            AddEntireEntityMarker(sb);
            this.projectionPaths.Add(sb);
        }

        internal IEnumerable<string> ExpandPaths
        {
            get
            {
                return this.expandPaths.Where<StringBuilder>(delegate (StringBuilder s) {
            return (s.Length > 0);
        }).Select<StringBuilder, string>(delegate (StringBuilder s) {
            return s.ToString();
        }).Distinct<string>();

            }
        }

        internal ParameterExpression ParamExpressionInScope
        {
            get
            {
                Debug.Assert(this.parameterExpressions.Count > 0);
                return this.parameterExpressions.Peek();
            }
        }

        internal IEnumerable<string> ProjectionPaths
        {
            get
            {
                return this.projectionPaths.Where<StringBuilder>(delegate(StringBuilder s)
                {
                    return (s.Length > 0);
                }).Select<StringBuilder, string>(delegate(StringBuilder s)
                {
                    return s.ToString();
                }).Distinct<string>();
            }
        }
    }
}

