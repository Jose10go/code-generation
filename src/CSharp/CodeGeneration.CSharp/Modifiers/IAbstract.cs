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
        public interface IAbstract<TCommand>
            where TCommand:Core.ICommand
        {
            SyntaxToken Abstract { get; set; }
            TCommand MakeAbstract() 
            {
                SyntaxFactory.Token(SyntaxKind.AbstractKeyword);
                return (TCommand)this;
            }
        }
    }
}
