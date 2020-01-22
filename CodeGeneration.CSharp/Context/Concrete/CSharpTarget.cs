using System;
using System.Collections.Generic;
using System.Linq;
using CodeGen.Context;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
namespace CodeGen.CSharp.Context
{
    public abstract partial class CSharpContext<TProcessEntity> : CodeGenContext<Solution, CSharpSyntaxNode, TProcessEntity>
    {
        public class CSharpTarget<TSyntaxNode> : ICSharpTarget<TSyntaxNode> where TSyntaxNode : CSharpSyntaxNode
        {
            public Func<TSyntaxNode, bool> WhereSelector { get; set; }
            public Core.ICodeGenerationEngine CodeGenerationEngine { get; set ; }

            public IEnumerable<TSyntaxNode> Select(CSharpSyntaxNode root)
            {
                return root.DescendantNodes().OfType<TSyntaxNode>().Where(WhereSelector);
            }
        }
    }
}