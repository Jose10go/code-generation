using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
namespace CodeGen.Context.CSharp
{
    public partial class ICSharpContext<TProcessEntity> : CodeGenContext<Solution, CSharpSyntaxNode, TProcessEntity>
    {
        public interface ICSharpTargetBuilder<TNode> : ITargetBuilder<TNode>
            where TNode : CSharpSyntaxNode
        {
        }
    }
}
