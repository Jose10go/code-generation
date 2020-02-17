using CodeGen.Attributes;
using CodeGen.Context;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
namespace CodeGen.CSharp.Context
{
    public abstract partial class CSharpContext<TProcessEntity> : CodeGenContext<Project, CSharpSyntaxNode, ISymbol, TProcessEntity>
    {
        [CommandBuilderModifier]
        public interface IWithAccessModifier<TCommandBuilder,TNode>
            where TCommandBuilder:Core.ICommand
            where TNode:CSharpSyntaxNode                    
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
