using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Editing;
using CodeGen.Attributes;

namespace CodeGen.Commands.Handlers.DocumentEditing
{
    public class MethodCloneCommandHandler : RoslynDocumentEditorCommandHandler<CloneCommand<MethodDeclarationSyntax, CSharpSyntaxNode, DocumentEditor>, MethodDeclarationSyntax>
    {
        public override CloneCommand<MethodDeclarationSyntax, CSharpSyntaxNode, DocumentEditor> Command { get; internal set; }

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