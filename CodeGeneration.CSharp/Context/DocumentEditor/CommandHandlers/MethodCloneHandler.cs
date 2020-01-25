using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Editing;
using CodeGen.Attributes;
using CodeGen.Core;
using CodeGen.Context;
using Microsoft.CodeAnalysis;

namespace CodeGen.CSharp.Context.DocumentEdit
{
    public partial class CSharpContextDocumentEditor : CSharpContext<DocumentEditor>
    {
        [CommandHandler]
        public class MethodCloneCommandHandler : ICommandHandler<IMethodClone,MethodDeclarationSyntax>
        {
            public Command<MethodDeclarationSyntax> Command { get ; set ; }

            public void ProcessDocument(DocumentEditor editor)
            {
                var target = Command.Target;
                var methodDeclFiltered = target.Select((CSharpSyntaxNode)editor.GetChangedRoot());
                dynamic cmd = Command;
                foreach (var methodDecl in methodDeclFiltered)
                {
                    var cloneMethodDecl = SyntaxFactory.MethodDeclaration(methodDecl.AttributeLists, methodDecl.Modifiers, methodDecl.ReturnType, methodDecl.ExplicitInterfaceSpecifier,
                        SyntaxFactory.Identifier(cmd.NewName(methodDecl)), methodDecl.TypeParameterList, methodDecl.ParameterList,
                        methodDecl.ConstraintClauses, methodDecl.Body, methodDecl.ExpressionBody);

                    editor.InsertAfter(methodDecl, cloneMethodDecl);
                }
            }
        }
    }
}