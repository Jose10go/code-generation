using CodeGen.Attributes;
using CodeGen.Context;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
namespace CodeGen.CSharp.Context
{
    public abstract partial class CSharpContext : CodeGenContext<Project, CSharpSyntaxNode,CompilationUnitSyntax, ISymbol>
    {
        [CommandModifier]
        public interface ISealed<TCommandBuilder>
            where TCommandBuilder:Core.ICommand
        {
            SyntaxToken Sealed { get; set; }
            TCommandBuilder MakeSealed() 
            {
                Sealed=SyntaxFactory.Token(SyntaxKind.SealedKeyword);
                return (TCommandBuilder)this;
            }
        }
    }
}
