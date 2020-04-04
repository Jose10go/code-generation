using CodeGen.Attributes;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Editing;

namespace CodeGen.CSharp.Context.DocumentEdit
{
    public partial class CSharpContextDocumentEditor : CSharpContext<DocumentEditor>
    {
        [CommandHandler]
        public class CreateNamespaceCommandHandler :CommandHandler<ICreateNamespace,CompilationUnitSyntax> 
        {
            public CreateNamespaceCommandHandler(ICreateNamespace command) : base(command)
            {
            }

            public override void ProccessNode(CompilationUnitSyntax node, DocumentEditor documentEditor)
            {
                documentEditor.InsertMembers(node,0,
                                                new[]{SyntaxFactory.NamespaceDeclaration(SyntaxFactory.ParseName(Command.Name))});
            }
        }
    }
}