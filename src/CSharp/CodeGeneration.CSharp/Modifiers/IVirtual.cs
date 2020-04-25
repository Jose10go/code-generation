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
        public interface IVirtual<TCommandBuilder>
            where TCommandBuilder:Core.ICommand
        {
            SyntaxToken Virtual { get; set; }
            TCommandBuilder MakeVirtual() 
            {
                Virtual=SyntaxFactory.Token(SyntaxKind.VirtualKeyword);
                return (TCommandBuilder)this;
            }
        }
    }
}
