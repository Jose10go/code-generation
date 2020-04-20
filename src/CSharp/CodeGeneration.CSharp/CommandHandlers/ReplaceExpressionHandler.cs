using CodeGen.Attributes;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Editing;
using System;
using CodeGen.Context;
using Microsoft.CodeAnalysis.CSharp;

namespace CodeGen.CSharp.Context
{
    public abstract partial class CSharpContext : CodeGenContext<Project, CSharpSyntaxNode, CompilationUnitSyntax, ISymbol>
    {
        [CommandHandler]
        public class ReplaceExpressionCommandHandler<Exp> : CommandHandler<IReplaceExpression<Exp>,Exp,ExpressionSyntax>
            where Exp:ExpressionSyntax
        {
            public ReplaceExpressionCommandHandler(IReplaceExpression<Exp> command) : base(command)
            {
            }

            protected override ExpressionSyntax ProccessNode(Exp node, DocumentEditor documentEditor,Guid id)
            {
                var newNode= this.Command.NewExpression
                                         .WithAdditionalAnnotations(new SyntaxAnnotation($"{id}"));

                documentEditor.ReplaceNode(node,newNode);
                return newNode;
            }
        }
    }
}