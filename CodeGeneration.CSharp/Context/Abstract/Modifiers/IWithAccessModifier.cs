using CodeGen.Attributes;
using CodeGen.Context;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
namespace CodeGen.CSharp.Context
{
    public abstract partial class CSharpContext<TProcessEntity> : CodeGenContext<Solution, CSharpSyntaxNode, ISymbol, TProcessEntity>
    {
        [CommandBuilderModifier]
        public interface IWithAccessModifier<TCommandBuilder,TNode>
            where TCommandBuilder:Core.ICommandBuilder
            where TNode:CSharpSyntaxNode                    
        {
            SyntaxTokenList Modifiers { get; set; }
            TCommandBuilder MakePublic() 
            {
                Modifiers.Add(SyntaxFactory.Token(SyntaxKind.PublicKeyword));
                return (TCommandBuilder)this;
            }
            TCommandBuilder MakeInternal()
            {
                Modifiers.Add(SyntaxFactory.Token(SyntaxKind.InternalKeyword));
                return (TCommandBuilder)this;
            }
            TCommandBuilder MakePrivate()
            {
                Modifiers.Add(SyntaxFactory.Token(SyntaxKind.PrivateKeyword));
                return (TCommandBuilder)this;
            }
            TCommandBuilder MakeProtected()
            {
                Modifiers.Add(SyntaxFactory.Token(SyntaxKind.ProtectedKeyword));
                return (TCommandBuilder)this;
            }
        }
    }
}
