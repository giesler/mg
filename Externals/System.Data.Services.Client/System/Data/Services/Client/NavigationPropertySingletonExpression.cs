namespace System.Data.Services.Client
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Linq;
    using System.Linq.Expressions;

    internal class NavigationPropertySingletonExpression : ResourceExpression
    {
        private readonly Expression memberExpression;
        private readonly Type resourceType;

        internal NavigationPropertySingletonExpression(Type type, Expression source, Expression memberExpression, Type resourceType, List<string> expandPaths, CountOption countOption, Dictionary<ConstantExpression, ConstantExpression> customQueryOptions, ProjectionQueryOptionExpression projection) : base(source, (ExpressionType) 0x2712, type, expandPaths, countOption, customQueryOptions, projection)
        {
            Debug.Assert(memberExpression != null, "memberExpression != null");
            Debug.Assert(resourceType != null, "resourceType != null");
            this.memberExpression = memberExpression;
            this.resourceType = resourceType;
        }

        internal override ResourceExpression CreateCloneWithNewType(Type type)
        {
            return new NavigationPropertySingletonExpression(type, base.source, this.MemberExpression, TypeSystem.GetElementType(type), this.ExpandPaths.ToList<string>(), this.CountOption, this.CustomQueryOptions.ToDictionary<KeyValuePair<ConstantExpression, ConstantExpression>, ConstantExpression, ConstantExpression>(delegate(KeyValuePair<ConstantExpression, ConstantExpression> kvp)
            {
                return kvp.Key;
            }, delegate(KeyValuePair<ConstantExpression, ConstantExpression> kvp)
            {
                return kvp.Value;
            }), base.Projection);
        }

        internal override bool HasQueryOptions
        {
            get
            {
                return ((((this.ExpandPaths.Count > 0) || (this.CountOption == CountOption.InlineAll)) || (this.CustomQueryOptions.Count > 0)) || (base.Projection != null));
            }
        }

        internal override bool IsSingleton
        {
            get
            {
                return true;
            }
        }

        internal System.Linq.Expressions.MemberExpression MemberExpression
        {
            get
            {
                return (System.Linq.Expressions.MemberExpression) this.memberExpression;
            }
        }

        internal override Type ResourceType
        {
            get
            {
                return this.resourceType;
            }
        }
    }
}

