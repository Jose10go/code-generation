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
        public interface IPartial<TCommandBuilder>
            where TCommandBuilder:Core.ICommand
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
