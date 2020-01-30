using System;
using System.Collections.Generic;
using System.Linq;
using CodeGen.Context;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
namespace CodeGen.CSharp.Context
{
    public abstract partial class CSharpContext<TProcessEntity> : CodeGenContext<Solution, CSharpSyntaxNode, ISymbol, TProcessEntity>
    {
        public class CSharpTarget<TSyntaxNode> : ICSharpTarget<TSyntaxNode> where TSyntaxNode : CSharpSyntaxNode
        {
            public ICodeGenerationEngine CodeGenerationEngine { get; set ; }
            public Func<ISymbol, TSyntaxNode, bool> WhereSelector { get ; set ; }

            public IEnumerable<TSyntaxNode> Select(CSharpSyntaxNode root,Func<TSyntaxNode,ISymbol> semanticModelSelector)
            {
                return root.DescendantNodes().OfType<TSyntaxNode>().Where(x=>WhereSelector(semanticModelSelector(x),x));
            }
        }
    }
}