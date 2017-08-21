namespace System.Data.Services.Client
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Linq.Expressions;
    using System.Runtime.CompilerServices;

    internal class ProjectionPathBuilder
    {
        private readonly Stack<bool> entityInScope = new Stack<bool>();
        private readonly Stack<Expression> parameterEntries = new Stack<Expression>();
        private readonly Stack<ParameterExpression> parameterExpressions = new Stack<ParameterExpression>();
        private readonly Stack<Expression> parameterExpressionTypes = new Stack<Expression>();
        private readonly Stack<Type> parameterProjectionTypes = new Stack<Type>();
        private readonly List<MemberInitRewrite> rewrites = new List<MemberInitRewrite>();

        internal ProjectionPathBuilder()
        {
        }

        internal void EnterLambdaScope(LambdaExpression lambda, Expression entry, Expression expectedType)
        {
            Debug.Assert(lambda != null, "lambda != null");
            Debug.Assert(lambda.Parameters.Count == 1, "lambda.Parameters.Count == 1");
            ParameterExpression item = lambda.Parameters[0];
            Type t = lambda.Body.Type;
            bool flag = ClientType.CheckElementTypeIsEntity(t);
            this.entityInScope.Push(flag);
            this.parameterExpressions.Push(item);
            this.parameterExpressionTypes.Push(expectedType);
            this.parameterEntries.Push(entry);
            this.parameterProjectionTypes.Push(t);
        }

        internal void EnterMemberInit(MemberInitExpression init)
        {
            bool item = ClientType.CheckElementTypeIsEntity(init.Type);
            this.entityInScope.Push(item);
        }

        internal Expression GetRewrite(Expression expression)
        {
            Debug.Assert(expression != null, "expression != null");
            List<string> list = new List<string>();
            while (expression.NodeType == ExpressionType.MemberAccess)
            {
                MemberExpression expression2 = (MemberExpression) expression;
                list.Add(expression2.Member.Name);
                expression = expression2.Expression;
            }
            foreach (MemberInitRewrite rewrite in this.rewrites)
            {
                if ((rewrite.Root != expression) || (list.Count != rewrite.MemberNames.Length))
                {
                    continue;
                }
                bool flag = true;
                for (int i = 0; (i < list.Count) && (i < rewrite.MemberNames.Length); i++)
                {
                    if (list[(list.Count - i) - 1] != rewrite.MemberNames[i])
                    {
                        flag = false;
                        break;
                    }
                }
                if (flag)
                {
                    return rewrite.RewriteExpression;
                }
            }
            return null;
        }

        internal void LeaveLambdaScope()
        {
            this.entityInScope.Pop();
            this.parameterExpressions.Pop();
            this.parameterExpressionTypes.Pop();
            this.parameterEntries.Pop();
            this.parameterProjectionTypes.Pop();
        }

        internal void LeaveMemberInit()
        {
            this.entityInScope.Pop();
        }

        internal void RegisterRewrite(Expression root, string[] names, Expression rewriteExpression)
        {
            Debug.Assert(root != null, "root != null");
            Debug.Assert(names != null, "names != null");
            Debug.Assert(rewriteExpression != null, "rewriteExpression != null");
            MemberInitRewrite item = new MemberInitRewrite();
            item.Root = root;
            item.MemberNames = names;
            item.RewriteExpression = rewriteExpression;
            this.rewrites.Add(item);
            this.parameterEntries.Push(rewriteExpression);
        }

        internal void RevokeRewrite(Expression root, string[] names)
        {
            Debug.Assert(root != null, "root != null");
            for (int i = 0; i < this.rewrites.Count; i++)
            {
                if ((this.rewrites[i].Root != root) || (names.Length != this.rewrites[i].MemberNames.Length))
                {
                    continue;
                }
                bool flag = true;
                for (int j = 0; j < names.Length; j++)
                {
                    if (names[j] != this.rewrites[i].MemberNames[j])
                    {
                        flag = false;
                        break;
                    }
                }
                if (flag)
                {
                    this.rewrites.RemoveAt(i);
                    this.parameterEntries.Pop();
                    break;
                }
            }
        }

        public override string ToString()
        {
            string str = "ProjectionPathBuilder: ";
            if (this.parameterExpressions.Count == 0)
            {
                return (str + "(empty)");
            }
            object obj2 = str;
            return string.Concat(new object[] { obj2, "entity:", this.CurrentIsEntity, " param:", this.ParameterEntryInScope });
        }

        internal bool CurrentIsEntity
        {
            get
            {
                return this.entityInScope.Peek();
            }
        }

        internal Expression ExpectedParamTypeInScope
        {
            get
            {
                Debug.Assert(this.parameterExpressionTypes.Count > 0, "this.parameterExpressionTypes.Count > 0");
                return this.parameterExpressionTypes.Peek();
            }
        }

        internal bool HasRewrites
        {
            get
            {
                return (this.rewrites.Count > 0);
            }
        }

        internal Expression LambdaParameterInScope
        {
            get
            {
                return this.parameterExpressions.Peek();
            }
        }

        internal Expression ParameterEntryInScope
        {
            get
            {
                return this.parameterEntries.Peek();
            }
        }

        internal class MemberInitRewrite
        {
            public string[] MemberNames { get; set; }
            public Expression RewriteExpression { get; set; }
            public Expression Root { get; set; }
            //[CompilerGenerated]
            //private string[] <MemberNames>k__BackingField;
            //[CompilerGenerated]
            //private Expression <RewriteExpression>k__BackingField;
            //[CompilerGenerated]
            //private Expression <Root>k__BackingField;

            //internal string[] MemberNames
            //{
            //    [CompilerGenerated]
            //    get
            //    {
            //        return this.<MemberNames>k__BackingField;
            //    }
            //    [CompilerGenerated]
            //    set
            //    {
            //        this.<MemberNames>k__BackingField = value;
            //    }
            //}

            //internal Expression RewriteExpression
            //{
            //    [CompilerGenerated]
            //    get
            //    {
            //        return this.<RewriteExpression>k__BackingField;
            //    }
            //    [CompilerGenerated]
            //    set
            //    {
            //        this.<RewriteExpression>k__BackingField = value;
            //    }
            //}

            //internal Expression Root
            //{
            //    [CompilerGenerated]
            //    get
            //    {
            //        return this.<Root>k__BackingField;
            //    }
            //    [CompilerGenerated]
            //    set
            //    {
            //        this.<Root>k__BackingField = value;
            //    }
            //}
        }
    }
}

