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
        public class MethodCloneCommandHandler : CommandHandler<IMethodClone,MethodDeclarationSyntax>
        {
            public MethodCloneCommandHandler(IMethodClone command) :base(command)
            {
            }

            public override MethodDeclarationSyntax ProccessNode(MethodDeclarationSyntax targetNode,DocumentEditor documentEditor,ICodeGenerationEngine engine)
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
                var methodNode = targetNode.WithIdentifier(SyntaxFactory.ParseToken(Command.NewName(targetNode)))
                                               .WithAttributeLists(Command.Attributes)
                                               .WithBody(Command.Body)
                                               .WithModifiers(modifiers);
                documentEditor.InsertAfter(targetNode,methodNode);

                return methodNode;
            }
        }
    }
}