using CodeGen.Attributes;
using CodeGen.Context;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
namespace CodeGen.CSharp.Context
{
    public abstract partial class CSharpContext<TProcessEntity> : CodeGenContext<Project, CSharpSyntaxNode,CompilationUnitSyntax, ISymbol, TProcessEntity>
    {
        [CommandModifier]
        public interface IAbstract<TCommand,TNode>
            where TCommand:Core.ICommand
            where TNode:CSharpSyntaxNode                    
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
