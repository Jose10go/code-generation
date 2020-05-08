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
        public interface IOn<TCommand>
             where TCommand:Core.ICommand,IOn<TCommand>
        {
            CSharpSyntaxNode OnNode { get; set; }
        }
        
        [CommandModifier]
        public interface IOn<TCommand,TSyntaxNode>:IOn<TCommand>
            where TCommand:Core.ICommand,IOn<TCommand>
            where TSyntaxNode:CSharpSyntaxNode
        {
            TCommand On(TSyntaxNode node) 
            {
                this.OnNode = node;
                return (TCommand)this;
            }
        }
    }
}
