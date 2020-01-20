﻿//using CodeGen.Attributes;
//using CodeGen.Core;
//using Microsoft.CodeAnalysis.CSharp;
//using Microsoft.CodeAnalysis.CSharp.Syntax;
//using Microsoft.CodeAnalysis.Editing;

//namespace CodeGen.CSharp.Context.DocumentEdit
//{
//    public partial class CSharpContextDocumentEditor : CSharpContext<DocumentEditor>
//    {
//        [CommandHandler]
//        public class ClassCloneCommandHandler : RoslynDocumentEditorCommandHandler<ClassDeclarationSyntax>
//        {
//            public override Command Command { get; set; }

//            public override void ProcessDocument(DocumentEditor ProcessEntity)
//            {
//                var target = Command.Target as ITarget<ClassDeclarationSyntax>;
//                var classDeclFiltered = target.Select((CSharpSyntaxNode)ProcessEntity.GetChangedRoot());
//                dynamic cmd = Command;
//                foreach (var classDecl in classDeclFiltered)
//                {
//                    var cloneClassDecl = SyntaxFactory.ClassDeclaration(classDecl.AttributeLists, classDecl.Modifiers, SyntaxFactory.Identifier(cmd.NewName(classDecl)),
//                                    classDecl.TypeParameterList, classDecl.BaseList, classDecl.ConstraintClauses, classDecl.Members);
//                    ProcessEntity.InsertAfter(classDecl, cloneClassDecl);
//                }
//            }
//        }
//    }
//}