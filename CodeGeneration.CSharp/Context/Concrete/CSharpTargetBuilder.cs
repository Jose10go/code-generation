using CodeGen.Context;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using System;

namespace CodeGen.CSharp.Context
{
    public abstract partial class CSharpContext<TProcessEntity> : CodeGenContext<Solution, CSharpSyntaxNode, ISymbol, TProcessEntity>
    {
        public class CSharpTargetBuilder<TNode> : TargetBuilder<TNode>
            where TNode : CSharpSyntaxNode
        {
            public CSharpTargetBuilder(ICodeGenerationEngine engine,ITarget<TNode> target):base(engine,target)
            {
            }
        }

        public class ChainCSharpTargetBuilder<TNode> : ChainTargetBuilder<TNode>
           where TNode : CSharpSyntaxNode
        {
            public ChainCSharpTargetBuilder(ICodeGenerationEngine engine, ITarget<TNode> target) : base(engine, target)
            {
            }
        }
    }

}