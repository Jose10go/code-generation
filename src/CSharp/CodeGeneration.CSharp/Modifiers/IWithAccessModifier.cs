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
        public interface IWithAccessModifier<TCommandBuilder>
            where TCommandBuilder:Core.ICommand
        {
            SyntaxToken Modifiers { get; set; }
            TCommandBuilder MakePublic() 
            {
                Modifiers=SyntaxFactory.Token(SyntaxKind.PublicKeyword);
                return (TCommandBuilder)this;
            }
            TCommandBuilder MakeInternal()
            {
                Modifiers=SyntaxFactory.Token(SyntaxKind.InternalKeyword);
                return (TCommandBuilder)this;
            }
            TCommandBuilder MakePrivate()
            {
                Modifiers=SyntaxFactory.Token(SyntaxKind.PrivateKeyword);
                return (TCommandBuilder)this;
            }
            TCommandBuilder MakeProtected()
            {
                Modifiers=SyntaxFactory.Token(SyntaxKind.ProtectedKeyword);
                return (TCommandBuilder)this;
            }
        }
    }
}
