using CodeGen.Attributes;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Editing;

namespace CodeGen.CSharp.Context.DocumentEdit
{
    public partial class CSharpContextDocumentEditor : CSharpContext<DocumentEditor>
    {
        [CommandHandler]
        public class CreateNamespaceCommandHandler :CommandHandler<ICreateNamespace,CompilationUnitSyntax,NamespaceDeclarationSyntax> 
        {
            public CreateNamespaceCommandHandler(ICreateNamespace command) : base(command)
            {
            }

            public override NamespaceDeclarationSyntax ProccessNode(CompilationUnitSyntax targetNode, DocumentEditor documentEditor,ICodeGenerationEngine engine)
            {
                var namespaceNode=SyntaxFactory.NamespaceDeclaration(SyntaxFactory.ParseName(Command.Name));
                documentEditor.InsertMembers(targetNode,0,new[]{namespaceNode});
                return namespaceNode;
            }
        }
    }
}