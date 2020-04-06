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

            public override SingleTarget<MethodDeclarationSyntax> ProccessNode(SingleTarget<MethodDeclarationSyntax> target,DocumentEditor documentEditor,ICodeGenerationEngine engine)
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
                var methodNode = target.Node.WithIdentifier(SyntaxFactory.ParseToken(Command.NewName(target.Node)))
                                               .WithAttributeLists(Command.Attributes)
                                               .WithBody(Command.Body)
                                               .WithModifiers(modifiers);
                documentEditor.InsertAfter(target.Node,methodNode);
                return new CSharpSingleTarget<MethodDeclarationSyntax>(engine,documentEditor.SemanticModel,methodNode);
            }
        }
    }
}