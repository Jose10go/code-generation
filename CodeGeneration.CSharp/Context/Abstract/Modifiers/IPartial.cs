using CodeGen.Attributes;
using CodeGen.Context;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
namespace CodeGen.CSharp.Context
{
    public abstract partial class CSharpContext<TProcessEntity> : CodeGenContext<Solution, CSharpSyntaxNode, ISymbol, TProcessEntity>
    {
        [CommandBuilderModifier]
        public interface IPartial<TCommandBuilder,TNode>
            where TCommandBuilder:Core.ICommandBuilder
            where TNode:CSharpSyntaxNode                    
        {
            SyntaxToken Partial { get; set; }
            TCommandBuilder MakePartial() 
            {
                Partial=SyntaxFactory.Token(SyntaxKind.PartialKeyword);
                return (TCommandBuilder)this;
            }
        }
    }
}
