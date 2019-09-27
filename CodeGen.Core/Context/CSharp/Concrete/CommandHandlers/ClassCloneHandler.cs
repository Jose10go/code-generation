using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Editing;

namespace CodeGen.Context.CSharp
{
    public partial class CSharpContext<TProcessEntity> : CodeGen.Context.CodeGenContext<Solution, CSharpSyntaxNode, TProcessEntity>
    {
        public class ClassCloneCommandHandler : RoslynDocumentEditorCommandHandler<CloneCommand<ClassDeclarationSyntax>, ClassDeclarationSyntax>
        {
            public override CloneCommand<ClassDeclarationSyntax> Command { get; set; }

            public override DocumentEditor ProcessDocument(DocumentEditor editor)
            {
                var classDeclFiltered = Target.Select((CSharpSyntaxNode)editor.GetChangedRoot());

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