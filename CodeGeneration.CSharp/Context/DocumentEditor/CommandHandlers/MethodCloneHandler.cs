using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Editing;
using CodeGen.Attributes;

namespace CodeGen.CSharp.Context.DocumentEdit
{
    public partial class CSharpContextDocumentEditor : CSharpContext<DocumentEditor>
    {
        [CommandHandler]
        public class MethodCloneCommandHandler : CommandHandler<IMethodClone,MethodDeclarationSyntax>
        {
            public MethodCloneCommandHandler(IMethodClone command) :base(command)
            {
            }

            public override void ProccesNode(MethodDeclarationSyntax node,DocumentEditor documentEditor)
            {
                documentEditor.InsertAfter(node,
                                           node.WithIdentifier(SyntaxFactory.ParseToken(Command.NewName(node)))
                                               .WithAttributeLists(Command.Attributes)
                                               .WithBody(Command.Body)
                                               .WithModifiers(Command.Modifiers));
            }
        }
    }
}