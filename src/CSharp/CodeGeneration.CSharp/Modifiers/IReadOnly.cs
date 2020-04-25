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
        public interface IReadOnly<TCommandBuilder>
            where TCommandBuilder:Core.ICommand
        {
            SyntaxToken ReadOnly { get; set; }
            TCommandBuilder MakeReadOnly() 
            {
                ReadOnly = SyntaxFactory.Token(SyntaxKind.ReadOnlyKeyword);
                return (TCommandBuilder)this;
            }
        }
    }
}
