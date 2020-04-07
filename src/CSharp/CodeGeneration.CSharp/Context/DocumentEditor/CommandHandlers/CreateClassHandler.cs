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
        public class CreateClassCommandHandler :CommandHandler<ICreateClass,NamespaceDeclarationSyntax,ClassDeclarationSyntax> 
        {
            public CreateClassCommandHandler(ICreateClass command) : base(command)
            {
            }

            public override ClassDeclarationSyntax ProccessNode(NamespaceDeclarationSyntax targetNode, DocumentEditor documentEditor,ICodeGenerationEngine engine)
            {
                var modifiers = new SyntaxTokenList();
                if (Command.Modifiers != default)
                    modifiers=modifiers.Add(Command.Modifiers);
                if (Command.Abstract != default)
                    modifiers = modifiers.Add(Command.Abstract);
                if (Command.Static != default)
                    modifiers = modifiers.Add(Command.Static);
                if (Command.Partial != default)
                    modifiers = modifiers.Add(Command.Partial);
                var classNode = SyntaxFactory.ClassDeclaration(Command.Name)
                                             .WithAttributeLists(Command.Attributes)
                                             .WithModifiers(modifiers);
                documentEditor.InsertMembers(targetNode,0,new[]{classNode});
                return classNode;
            }
        }
    }
}