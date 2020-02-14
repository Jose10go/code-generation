using CodeGen.Context;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
namespace CodeGen.CSharp.Context
{
    public abstract partial class CSharpContext<TProcessEntity> : CodeGenContext<Project, CSharpSyntaxNode, ISymbol, TProcessEntity>
    {
        public interface ICSharpTarget<TNode> : ITarget<TNode>
            where TNode : CSharpSyntaxNode
        {

        }
    }
}
