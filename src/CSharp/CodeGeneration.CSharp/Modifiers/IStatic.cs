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
        public interface IStatic<TCommandBuilder>
            where TCommandBuilder:Core.ICommand
        {
            SyntaxToken Static { get; set; }
            TCommandBuilder MakeStatic() 
            {
                Static=SyntaxFactory.Token(SyntaxKind.StaticKeyword);
                return (TCommandBuilder)this;
            }
        }
    }
}
