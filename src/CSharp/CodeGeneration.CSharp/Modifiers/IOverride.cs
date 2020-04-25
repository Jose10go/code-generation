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
        public interface IOverride<TCommandBuilder>
            where TCommandBuilder:Core.ICommand
        {
            SyntaxToken Override { get; set; }
            TCommandBuilder MakeOverride() 
            {
                Override=SyntaxFactory.Token(SyntaxKind.OverrideKeyword);
                return (TCommandBuilder)this;
            }
        }
    }
}
