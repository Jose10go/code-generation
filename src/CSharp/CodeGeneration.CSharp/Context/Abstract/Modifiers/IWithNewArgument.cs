using System;
using CodeGen.Attributes;
using CodeGen.Context;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CodeGen.CSharp.Context
{
    public abstract partial class CSharpContext<TProcessEntity> : CodeGenContext<Project, CSharpSyntaxNode, CompilationUnitSyntax, ISymbol, TProcessEntity>
    {
        [CommandModifier]
        public interface IWith<TCommandBuilder,TNode>
            where TCommandBuilder:Core.ICommand
            where TNode:CSharpSyntaxNode                    
        {
            Func<ExpressionSyntax,ExpressionSyntax> NewExpression { get; set; }
            TCommandBuilder With(Func<ExpressionSyntax, ExpressionSyntax> NewExpression) 
            {
                this.NewExpression=NewExpression;
                return (TCommandBuilder)this;
            } 
        }
    }
}
