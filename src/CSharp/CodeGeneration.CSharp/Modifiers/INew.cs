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
        public interface INew<TCommandBuilder>
            where TCommandBuilder:Core.ICommand
        {
            SyntaxToken New{ get; set; }
            TCommandBuilder MakeNew() 
            {
                New=SyntaxFactory.Token(SyntaxKind.NewKeyword);
                return (TCommandBuilder)this;
            }
        }
    }
}
