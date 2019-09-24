using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;


namespace CodeGen.Context.CSharp
{
    public partial class ICSharpContext<TProcessEntity> : CodeGenContext<Solution, CSharpSyntaxNode, TProcessEntity>
    {
        public interface ICSharpTarget<TNode> : ITarget<TNode>
            where TNode : CSharpSyntaxNode
        {

        }
    }
}
