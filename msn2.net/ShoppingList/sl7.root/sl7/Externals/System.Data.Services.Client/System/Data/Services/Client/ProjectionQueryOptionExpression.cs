namespace System.Data.Services.Client
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Linq.Expressions;

    internal class ProjectionQueryOptionExpression : QueryOptionExpression
    {
        private readonly LambdaExpression lambda;
        private readonly List<string> paths;

        internal ProjectionQueryOptionExpression(Type type, LambdaExpression lambda, List<string> paths) : base((ExpressionType) 0x2718, type)
        {
            Debug.Assert(type != null, "type != null");
            Debug.Assert(lambda != null, "lambda != null");
            Debug.Assert(paths != null, "paths != null");
            this.lambda = lambda;
            this.paths = paths;
        }

        internal List<string> Paths
        {
            get
            {
                return this.paths;
            }
        }

        internal LambdaExpression Selector
        {
            get
            {
                return this.lambda;
            }
        }
    }
}

