using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Editing;
using CodeGen.Attributes;
using System;

namespace CodeGen.Context.CSharp.DocumentEdit
{
    public partial class CSharpContextDocumentEditor : CSharpContext<DocumentEditor>
    {
        [CommandHandler]
        public class MethodCloneCommandHandler : RoslynDocumentEditorCommandHandler<MethodCloneCommand, MethodDeclarationSyntax>
        {
            public override MethodCloneCommand Command { get; set; }

            public override DocumentEditor ProcessDocument(DocumentEditor editor)
            {
                var target = Command.Target as ITarget<MethodDeclarationSyntax>;
                var methodDeclFiltered = target.Select((CSharpSyntaxNode)editor.GetChangedRoot());

                foreach (var methodDecl in methodDeclFiltered)
                {
                    var cloneMethodDecl = SyntaxFactory.MethodDeclaration(methodDecl.AttributeLists, methodDecl.Modifiers, methodDecl.ReturnType, methodDecl.ExplicitInterfaceSpecifier,
                        SyntaxFactory.Identifier(Command.NewName(methodDecl)), methodDecl.TypeParameterList, methodDecl.ParameterList,
                        methodDecl.ConstraintClauses, methodDecl.Body, methodDecl.ExpressionBody);

                    editor.InsertAfter(methodDecl, cloneMethodDecl);
                }
                return editor;
            }
        }
    }
}