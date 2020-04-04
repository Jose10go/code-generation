using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Editing;
using CodeGen.Attributes;
using Microsoft.CodeAnalysis;

namespace CodeGen.CSharp.Context.DocumentEdit
{
    public partial class CSharpContextDocumentEditor : CSharpContext<DocumentEditor>
    {
        [CommandHandler]
        public class CreateMethodCommandHandler : CommandHandler<ICreateMethod,ClassDeclarationSyntax>
        {
            public CreateMethodCommandHandler(ICreateMethod command) :base(command)
            {
            }

            public override void ProccessNode(ClassDeclarationSyntax node, DocumentEditor documentEditor)
            {
                var modifiers = new SyntaxTokenList();
                if (Command.Modifiers != default)
                    modifiers = modifiers.Add(Command.Modifiers);
                if (Command.Abstract != default)
                    modifiers = modifiers.Add(Command.Abstract);
                if (Command.Static != default)
                    modifiers = modifiers.Add(Command.Static);
                if (Command.Partial != default)
                    modifiers = modifiers.Add(Command.Partial);
                documentEditor.InsertMembers(node,0,
                                             new[]{ SyntaxFactory.MethodDeclaration(SyntaxFactory.ParseTypeName("void"),Command.Name)
                                                                 .WithAttributeLists(Command.Attributes)
                                                                 .WithBody(Command.Body)
                                                                 .WithModifiers(modifiers)});
            }
        }
    }
}