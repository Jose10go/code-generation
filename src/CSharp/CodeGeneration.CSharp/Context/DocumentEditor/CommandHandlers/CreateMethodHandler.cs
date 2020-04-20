using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Editing;
using CodeGen.Attributes;
using Microsoft.CodeAnalysis;
using System;

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

            protected override MethodDeclarationSyntax ProccessNode(ClassDeclarationSyntax node, DocumentEditor documentEditor,Guid id)
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
                                          .WithModifiers(modifiers)
                                          .WithAdditionalAnnotations(new SyntaxAnnotation($"{id}"));
                documentEditor.InsertMembers(node,0,new[]{method});
                return method;
            }
        }
    }
}