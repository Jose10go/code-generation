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
        public class CreateClassCommandHandler :CommandHandler<ICreateClass,NamespaceDeclarationSyntax> 
        {
            public CreateClassCommandHandler(ICreateClass command) : base(command)
            {
            }

            public override void ProccesNode(NamespaceDeclarationSyntax node, DocumentEditor documentEditor)
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
                documentEditor.InsertMembers(node,0,
                                                new[]{SyntaxFactory.ClassDeclaration(Command.Name)
                                                   .WithAttributeLists(Command.Attributes)
                                                   .WithModifiers(modifiers) });
            }
        }
    }
}