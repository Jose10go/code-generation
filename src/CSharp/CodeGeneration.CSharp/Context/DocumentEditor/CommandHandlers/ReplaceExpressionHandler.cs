using CodeGen.Attributes;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Editing;
using System;

namespace CodeGen.CSharp.Context.DocumentEdit
{
    public partial class CSharpContextDocumentEditor : CSharpContext<DocumentEditor>
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