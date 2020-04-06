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
        public class CreateMethodCommandHandler : CommandHandler<ICreateMethod,ClassDeclarationSyntax,MethodDeclarationSyntax>
        {
            public CreateMethodCommandHandler(ICreateMethod command) :base(command)
            {
            }

            public override SingleTarget<MethodDeclarationSyntax> ProccessNode(SingleTarget<ClassDeclarationSyntax> target, DocumentEditor documentEditor,ICodeGenerationEngine engine)
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
                var method = SyntaxFactory.MethodDeclaration(SyntaxFactory.ParseTypeName("void"), Command.Name)
                                          .WithAttributeLists(Command.Attributes)
                                          .WithBody(Command.Body)
                                          .WithModifiers(modifiers);

                documentEditor.InsertMembers(target.Node,0,new[]{method});
                return new CSharpSingleTarget<MethodDeclarationSyntax>(engine,documentEditor.SemanticModel,method);
            }
        }
    }
}