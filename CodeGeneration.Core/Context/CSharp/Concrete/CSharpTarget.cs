
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
            public Func<TSyntaxNode, bool> WhereSelector { get; set; }

            public IEnumerable<TSyntaxNode> Select(CSharpSyntaxNode root)
            {
                return root.DescendantNodes().OfType<TSyntaxNode>().Where(WhereSelector);
            }
        }
    }
}