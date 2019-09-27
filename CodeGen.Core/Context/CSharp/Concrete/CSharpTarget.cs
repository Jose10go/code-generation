
using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using static CodeGen.Context.CodeGenTypelessContext;

namespace CodeGen.Context.CSharp
{
    public partial class CSharpContext<TProcessEntity> : CodeGenContext<Solution, CSharpSyntaxNode, TProcessEntity>
    {
        public class CSharpTarget<TSyntaxNode> : ICSharpTarget<TSyntaxNode> where TSyntaxNode : CSharpSyntaxNode
        {
            public Type TargetNode => typeof(TSyntaxNode);

            public Func<TSyntaxNode, bool> WhereSelector { get; private set; }

            Func<object, bool> ITarget.WhereSelector => (o) => WhereSelector((TSyntaxNode)o);

            public CSharpTarget(Func<TSyntaxNode, bool> filter = null)
            {
                if (filter != null)
                {
                    WhereSelector = filter;
                }
                else
                {
                    WhereSelector = (node) => true;
                }
            }

            public IEnumerable<TSyntaxNode> Select(CSharpSyntaxNode root)
            {
                return root.DescendantNodes().OfType<TSyntaxNode>().Where(WhereSelector);
            }

            IEnumerable<object> ITarget.Select(object root)
            {
                return Select((CSharpSyntaxNode)root);
            }
        }
    }
}