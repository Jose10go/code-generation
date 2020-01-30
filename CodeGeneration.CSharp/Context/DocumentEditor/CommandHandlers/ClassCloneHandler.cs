using CodeGen.Attributes;
using CodeGen.Core;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Editing;

namespace CodeGen.CSharp.Context.DocumentEdit
{
    public partial class CSharpContextDocumentEditor : CSharpContext<DocumentEditor>
    {
        [CommandHandler]
        public class ClassCloneCommandHandler :CommandHandler<IClassClone,ClassDeclarationSyntax> 
        {
            public ClassCloneCommandHandler(IClassClone command) : base(command)
            {
            }

            public override void ProccesNode(ClassDeclarationSyntax node, DocumentEditor documentEditor)
            {
                documentEditor.InsertAfter(node,
                                           node.WithIdentifier(SyntaxFactory.ParseToken(Command.NewName(node)))
                                               .WithAttributeLists(Command.Attributes)
                                               .WithModifiers(Command.Modifiers));
            }
        }
    }
}