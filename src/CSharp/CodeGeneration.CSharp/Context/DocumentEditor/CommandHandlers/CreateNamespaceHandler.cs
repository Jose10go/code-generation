﻿using CodeGen.Attributes;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Editing;
using Microsoft.CodeAnalysis;
using System;

namespace CodeGen.CSharp.Context.DocumentEdit
{
    public partial class CSharpContextDocumentEditor : CSharpContext<DocumentEditor>
    {
        [CommandHandler]
        public class CreateNamespaceCommandHandler :CommandHandler<ICreateNamespace,CompilationUnitSyntax,NamespaceDeclarationSyntax> 
        {
            public CreateNamespaceCommandHandler(ICreateNamespace command) : base(command)
            {
            }

            protected override NamespaceDeclarationSyntax ProccessNode(CompilationUnitSyntax node, DocumentEditor documentEditor,Guid id)
            {
                var namespaceNode = SyntaxFactory.NamespaceDeclaration(SyntaxFactory.ParseName(Command.Name))
                                                 .WithAdditionalAnnotations(new SyntaxAnnotation($"{id}"));
                documentEditor.InsertMembers(node,0,new[]{namespaceNode});
                return namespaceNode;
            }
        }
    }
}