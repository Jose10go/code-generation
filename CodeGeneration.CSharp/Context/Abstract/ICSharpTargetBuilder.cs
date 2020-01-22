using CodeGen.Context;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;

namespace CodeGen.CSharp.Context
{
    public abstract partial class CSharpContext<TProcessEntity> : CodeGenContext<Solution, CSharpSyntaxNode, TProcessEntity>
    {
        public interface ICSharpTargetBuilder<TNode> : ITargetBuilder<TNode>
            where TNode : CSharpSyntaxNode
        {
        }

        public interface IChainCSharpTargetBuilder<TNode> : IChainTargetBuilder<TNode>
            where TNode : CSharpSyntaxNode
        {
        }
    }
}
