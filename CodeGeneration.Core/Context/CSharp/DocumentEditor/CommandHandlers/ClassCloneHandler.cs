using CodeGen.Attributes;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Editing;

namespace CodeGen.Context.CSharp.DocumentEdit
{
    public partial class CSharpContextDocumentEditor : CSharpContext<DocumentEditor>
    {
        [CommandHandler]
        public class ClassCloneCommandHandler : RoslynDocumentEditorCommandHandler<ClassCloneCommand,ClassDeclarationSyntax>
        {
            public override ClassCloneCommand Command { get; set; }

            public override DocumentEditor ProcessDocument(DocumentEditor editor)
            {
                var target = Command.Target as ITarget<ClassDeclarationSyntax>;
                var classDeclFiltered = target.Select((CSharpSyntaxNode)editor.GetChangedRoot());

                foreach (var classDecl in classDeclFiltered)
                {
                    var cloneClassDecl = SyntaxFactory.ClassDeclaration(classDecl.AttributeLists, classDecl.Modifiers, SyntaxFactory.Identifier(Command.NewName(classDecl)),
                                    classDecl.TypeParameterList, classDecl.BaseList, classDecl.ConstraintClauses, classDecl.Members);

                    editor.InsertAfter(classDecl, cloneClassDecl);
                }

                return editor;
            }
        }
    }
}