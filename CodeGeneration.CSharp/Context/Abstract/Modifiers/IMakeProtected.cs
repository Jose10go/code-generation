using CodeGen.Attributes;
using CodeGen.Context;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
namespace CodeGen.CSharp.Context
{
    public abstract partial class CSharpContext<TProcessEntity> : CodeGenContext<Solution, CSharpSyntaxNode, ISymbol, TProcessEntity>
    {
        [CommandBuilderModifier]
        public interface IMakeProtected<TCommandBuilder,TNode>
            where TCommandBuilder:Core.ICommandBuilder
            where TNode:CSharpSyntaxNode                    
        {
            SyntaxTokenList Modifiers { get; set; }
            TCommandBuilder MakeProtected() 
            {
                Modifiers.Add(SyntaxFactory.Token(SyntaxKind.ProtectedKeyword));
                return (TCommandBuilder)this;
            }
        }
    }
}
