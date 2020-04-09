using CodeGen.Attributes;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Editing;

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

            public override ExpressionSyntax ProccessNode(Exp node, DocumentEditor documentEditor,ICodeGenerationEngine engine)
            {
                documentEditor.ReplaceNode(node,this.Command.NewExpression);
                return this.Command.NewExpression;
            }
        }
    }
}