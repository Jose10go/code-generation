using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using System;
using static CodeGen.Context.CodeGenTypelessContext;

namespace CodeGen.Context.CSharp
{
    public partial class CSharpContext<TProcessEntity> : CodeGenContext<Solution, CSharpSyntaxNode, TProcessEntity>
    {
        public class CSharpTargetBuilder<TNode> : ICSharpTargetBuilder<TNode>
            where TNode : CSharpSyntaxNode
        {
            public Func<TNode, bool> WhereSelector { get ; set; }
        }

        public class ChainCSharpTargetBuilder<TNode> : IChainCSharpTargetBuilder<TNode>
           where TNode : CSharpSyntaxNode
        {
            public Func<TNode, bool> WhereSelector { get; set; }
        }
    }

}