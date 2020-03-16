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
        public interface IStatic<TCommandBuilder,TNode>
            where TCommandBuilder:Core.ICommand
            where TNode:CSharpSyntaxNode                    
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
