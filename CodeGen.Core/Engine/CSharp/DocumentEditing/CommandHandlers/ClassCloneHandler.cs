using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Editing;

namespace CodeGen.Commands.Handlers.DocumentEditing
{
    public class ClassCloneCommandHandler : RoslynDocumentEditorCommandHandler<CloneCommand<ClassDeclarationSyntax, CSharpSyntaxNode, DocumentEditor>, ClassDeclarationSyntax>
    {
        public override CloneCommand<ClassDeclarationSyntax, CSharpSyntaxNode, DocumentEditor> Command { get; internal set; }

        public override DocumentEditor ProcessDocument(DocumentEditor editor)
        {
            var classDeclFiltered = Target.Select((CSharpSyntaxNode)editor.GetChangedRoot());

            foreach(var classDecl in classDeclFiltered)
            {
                var cloneClassDecl = SyntaxFactory.ClassDeclaration(classDecl.AttributeLists, classDecl.Modifiers, SyntaxFactory.Identifier(Command.NewName(classDecl)),
                                classDecl.TypeParameterList, classDecl.BaseList, classDecl.ConstraintClauses, classDecl.Members);

                editor.InsertAfter(classDecl, cloneClassDecl);
            }

            return editor;
        }
    }
}