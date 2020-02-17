using CodeGen.Attributes;
using CodeGen.Context;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
namespace CodeGen.CSharp.Context
{
    public abstract partial class CSharpContext<TProcessEntity> : CodeGenContext<Project, CSharpSyntaxNode, ISymbol, TProcessEntity>
    {
        [CommandModifier]
        public interface IPartial<TCommandBuilder,TNode>
            where TCommandBuilder:Core.ICommand
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
