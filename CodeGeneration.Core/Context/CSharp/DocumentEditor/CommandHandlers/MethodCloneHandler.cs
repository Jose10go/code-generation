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
        public class MethodCloneCommandHandler : RoslynDocumentEditorCommandHandler<CloneCommand<MethodDeclarationSyntax>, MethodDeclarationSyntax>
        {
            public override CloneCommand<MethodDeclarationSyntax> Command { get; set; }

            public override DocumentEditor ProcessDocument(DocumentEditor editor)
            {
                var methodDeclFiltered = Target.Select((CSharpSyntaxNode)editor.GetChangedRoot());

                foreach (var methodDecl in methodDeclFiltered)
                {
                    var newAttrList = SyntaxFactory.AttributeList();
                    newAttrList = newAttrList.AddAttributes(SyntaxFactory.Attribute(SyntaxFactory.ParseName(nameof(CodeGenCreatedAttribute))));
                    var newAttrList1 = methodDecl.AttributeLists.Add(newAttrList);

                    var cloneMethodDecl = SyntaxFactory.MethodDeclaration(newAttrList1, methodDecl.Modifiers, methodDecl.ReturnType, methodDecl.ExplicitInterfaceSpecifier,
                        SyntaxFactory.Identifier(Command.NewName(methodDecl)), methodDecl.TypeParameterList, methodDecl.ParameterList,
                        methodDecl.ConstraintClauses, methodDecl.Body, methodDecl.ExpressionBody);

                    editor.InsertAfter(methodDecl, cloneMethodDecl);
                }
                return editor;
            }
        }
    }
}