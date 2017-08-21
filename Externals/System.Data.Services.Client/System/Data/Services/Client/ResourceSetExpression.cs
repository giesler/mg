﻿namespace System.Data.Services.Client
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Reflection;

    [DebuggerDisplay("ResourceSetExpression {Source}.{MemberExpression}")]
    internal class ResourceSetExpression : ResourceExpression
    {
        private Dictionary<PropertyInfo, ConstantExpression> keyFilter;
        private readonly Expression member;
        private readonly Type resourceType;
        private List<QueryOptionExpression> sequenceQueryOptions;
        private TransparentAccessors transparentScope;

        internal ResourceSetExpression(Type type, Expression source, Expression memberExpression, Type resourceType, List<string> expandPaths, CountOption countOption, Dictionary<ConstantExpression, ConstantExpression> customQueryOptions, ProjectionQueryOptionExpression projection) : base(source, (source != null) ? ((ExpressionType) 0x2711) : ((ExpressionType) 0x2710), type, expandPaths, countOption, customQueryOptions, projection)
        {
            Debug.Assert(type != null, "type != null");
            Debug.Assert(memberExpression != null, "memberExpression != null");
            Debug.Assert(resourceType != null, "resourceType != null");
            Debug.Assert(((source == null) && (memberExpression is ConstantExpression)) || ((source != null) && (memberExpression is System.Linq.Expressions.MemberExpression)), "source is null with constant entity set name, or not null with member expression");
            this.member = memberExpression;
            this.resourceType = resourceType;
            this.sequenceQueryOptions = new List<QueryOptionExpression>();
        }

        internal void AddSequenceQueryOption(QueryOptionExpression qoe)
        {
            Debug.Assert(qoe != null, "qoe != null");
            QueryOptionExpression previous = this.sequenceQueryOptions.Where<QueryOptionExpression>(delegate(QueryOptionExpression o)
            {
                return (o.GetType() == qoe.GetType());
            }).FirstOrDefault<QueryOptionExpression>();
            if (previous != null)
            {
                qoe = qoe.ComposeMultipleSpecification(previous);
                this.sequenceQueryOptions.Remove(previous);
            }
            this.sequenceQueryOptions.Add(qoe);

        }

        internal override ResourceExpression CreateCloneWithNewType(Type type)
        {
            ResourceSetExpression expression = new ResourceSetExpression(type, base.source, this.MemberExpression, TypeSystem.GetElementType(type), this.ExpandPaths.ToList<string>(), this.CountOption, this.CustomQueryOptions.ToDictionary<KeyValuePair<ConstantExpression, ConstantExpression>, ConstantExpression, ConstantExpression>(delegate(KeyValuePair<ConstantExpression, ConstantExpression> kvp)
            {
                return kvp.Key;
            }, delegate(KeyValuePair<ConstantExpression, ConstantExpression> kvp)
            {
                return kvp.Value;
            }), base.Projection);
            expression.keyFilter = this.keyFilter;
            expression.sequenceQueryOptions = this.sequenceQueryOptions;
            expression.transparentScope = this.transparentScope;
            return expression;

        }

        internal void OverrideInputReference(ResourceSetExpression newInput)
        {
            Debug.Assert(newInput != null, "Original resource set cannot be null");
            Debug.Assert(base.inputRef == null, "OverrideInputReference cannot be called if the target has already been referenced");
            InputReferenceExpression inputRef = newInput.inputRef;
            if (inputRef != null)
            {
                base.inputRef = inputRef;
                inputRef.OverrideTarget(this);
            }
        }

        internal FilterQueryOptionExpression Filter
        {
            get
            {
                return this.sequenceQueryOptions.OfType<FilterQueryOptionExpression>().SingleOrDefault<FilterQueryOptionExpression>();
            }
        }

        internal bool HasKeyPredicate
        {
            get
            {
                return (this.keyFilter != null);
            }
        }

        internal override bool HasQueryOptions
        {
            get
            {
                return ((((this.sequenceQueryOptions.Count > 0) || (this.ExpandPaths.Count > 0)) || ((this.CountOption == CountOption.InlineAll) || (this.CustomQueryOptions.Count > 0))) || (base.Projection != null));
            }
        }

        internal bool HasSequenceQueryOptions
        {
            get
            {
                return (this.sequenceQueryOptions.Count > 0);
            }
        }

        internal bool HasTransparentScope
        {
            get
            {
                return (this.transparentScope != null);
            }
        }

        internal override bool IsSingleton
        {
            get
            {
                return this.HasKeyPredicate;
            }
        }

        internal Dictionary<PropertyInfo, ConstantExpression> KeyPredicate
        {
            get
            {
                return this.keyFilter;
            }
            set
            {
                this.keyFilter = value;
            }
        }

        internal Expression MemberExpression
        {
            get
            {
                return this.member;
            }
        }

        internal OrderByQueryOptionExpression OrderBy
        {
            get
            {
                return this.sequenceQueryOptions.OfType<OrderByQueryOptionExpression>().SingleOrDefault<OrderByQueryOptionExpression>();
            }
        }

        internal override Type ResourceType
        {
            get
            {
                return this.resourceType;
            }
        }

        internal IEnumerable<QueryOptionExpression> SequenceQueryOptions
        {
            get
            {
                return this.sequenceQueryOptions.ToList<QueryOptionExpression>();
            }
        }

        internal SkipQueryOptionExpression Skip
        {
            get
            {
                return this.sequenceQueryOptions.OfType<SkipQueryOptionExpression>().SingleOrDefault<SkipQueryOptionExpression>();
            }
        }

        internal TakeQueryOptionExpression Take
        {
            get
            {
                return this.sequenceQueryOptions.OfType<TakeQueryOptionExpression>().SingleOrDefault<TakeQueryOptionExpression>();
            }
        }

        internal TransparentAccessors TransparentScope
        {
            get
            {
                return this.transparentScope;
            }
            set
            {
                this.transparentScope = value;
            }
        }

        [DebuggerDisplay("{ToString()}")]
        internal class TransparentAccessors
        {
            internal readonly string Accessor;
            internal readonly Dictionary<string, Expression> SourceAccessors;

            internal TransparentAccessors(string acc, Dictionary<string, Expression> sourceAccesors)
            {
                Debug.Assert(!string.IsNullOrEmpty(acc), "Set accessor cannot be null or empty");
                Debug.Assert(sourceAccesors != null, "sourceAccesors != null");
                this.Accessor = acc;
                this.SourceAccessors = sourceAccesors;
            }

            public override string ToString()
            {
                return (("SourceAccessors=[" + string.Join(",", this.SourceAccessors.Keys.ToArray<string>())) + "] ->* Accessor=" + this.Accessor);
            }
        }
    }
}

