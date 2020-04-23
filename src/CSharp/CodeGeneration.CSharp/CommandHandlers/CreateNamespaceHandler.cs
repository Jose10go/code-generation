using CodeGen.Attributes;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Editing;
using Microsoft.CodeAnalysis;
using System;
using CodeGen.Context;
using System.Linq;

namespace CodeGen.CSharp.Context
{
    public abstract partial class CSharpContext:CodeGenContext<Project,CSharpSyntaxNode,CompilationUnitSyntax,ISymbol>
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
                var newcompilationUnitNode = node.WithMembers(new SyntaxList<MemberDeclarationSyntax>(namespaceNode));
                if (Command.Namespaces != null)
                    newcompilationUnitNode = newcompilationUnitNode.WithUsings(new SyntaxList<UsingDirectiveSyntax>(Command.Namespaces.Select(n => SyntaxFactory.UsingDirective(SyntaxFactory.ParseName(n)))));
                
                documentEditor.ReplaceNode(node, newcompilationUnitNode);
                return namespaceNode;
            }
        }
    }
}