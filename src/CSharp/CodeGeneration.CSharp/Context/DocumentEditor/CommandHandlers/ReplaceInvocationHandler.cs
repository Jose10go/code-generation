using CodeGen.Attributes;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Editing;

namespace CodeGen.CSharp.Context.DocumentEdit
{
    public partial class CSharpContextDocumentEditor : CSharpContext<DocumentEditor>
    {
        [CommandHandler]
        public class ReplaceInvocationCommandHandler :CommandHandler<IReplaceInvocation, InvocationExpressionSyntax> 
        {
            public ReplaceInvocationCommandHandler(IReplaceInvocation command) : base(command)
            {
            }

            public override void ProccessNode(InvocationExpressionSyntax node, DocumentEditor documentEditor)
            {
                foreach (var (gen, pos) in Command.NewArguments) 
                {
                    var arg = node.ArgumentList.Arguments[pos];
                    documentEditor.ReplaceNode(arg,gen(node));
                }
            }
        }
    }
}