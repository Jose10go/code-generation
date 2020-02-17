using System;
using System.Collections.Generic;
using System.Linq;
using CodeGen.Context;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
namespace CodeGen.CSharp.Context
{
    public abstract partial class CSharpContext<TProcessEntity> : CodeGenContext<Project, CSharpSyntaxNode, ISymbol, TProcessEntity>
    {
        public class CSharpTarget<TSyntaxNode> : Target<TSyntaxNode> where TSyntaxNode : CSharpSyntaxNode
        {
            public CSharpTarget(ICodeGenerationEngine codeGenerationEngine) : base(codeGenerationEngine)
            {
            }
            public override IEnumerable<TSyntaxNode> Select(CSharpSyntaxNode root,Func<TSyntaxNode,ISymbol> semanticModelSelector)
            {
                return root.DescendantNodes().OfType<TSyntaxNode>().Where(x=>WhereSelector(semanticModelSelector(x),x));
            }
        }
    }
}