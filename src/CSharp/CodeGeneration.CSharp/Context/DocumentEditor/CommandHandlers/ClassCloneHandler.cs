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
        public class ClassCloneCommandHandler :CommandHandler<IClassClone,ClassDeclarationSyntax> 
        {
            public ClassCloneCommandHandler(IClassClone command) : base(command)
            {
            }

            public override SingleTarget<ClassDeclarationSyntax> ProccessNode(SingleTarget<ClassDeclarationSyntax> target, DocumentEditor documentEditor,ICodeGenerationEngine engine)
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
                var newNode = target.Node.WithIdentifier(SyntaxFactory.Identifier(Command.NewName(target.Node)))
                                       .WithAttributeLists(Command.Attributes)
                                       .WithModifiers(modifiers);
                documentEditor.InsertAfter(target.Node,newNode);
                return new CSharpSingleTarget<ClassDeclarationSyntax>(engine,documentEditor.SemanticModel,newNode);
            }
        }
    }
}