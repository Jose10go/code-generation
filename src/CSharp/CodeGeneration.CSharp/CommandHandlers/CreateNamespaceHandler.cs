using CodeGen.Attributes;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Editing;
using Microsoft.CodeAnalysis;
using CodeGen.Context;
using System.Linq;

namespace CodeGen.CSharp.Context
{
    public abstract partial class CSharpContext:CodeGenContext<Project,CSharpSyntaxNode,CompilationUnitSyntax,ISymbol>
    {
        [CommandHandler]
        public class CreateNamespaceCommandHandler :CommandHandler<ICreateNamespace> 
        {
            public CreateNamespaceCommandHandler(ICreateNamespace command) : base(command)
            {
            }

            public override void VisitCompilationUnit(CompilationUnitSyntax node)
            {
                var namespaceNode = SyntaxFactory.NamespaceDeclaration(SyntaxFactory.ParseName(Command.Name))
                                                 .WithAdditionalAnnotations(new SyntaxAnnotation($"{Id}"));
                var newcompilationUnitNode = node.WithMembers(new SyntaxList<MemberDeclarationSyntax>(namespaceNode));
                if (Command.Namespaces != null)
                    newcompilationUnitNode = newcompilationUnitNode.WithUsings(new SyntaxList<UsingDirectiveSyntax>(Command.Namespaces.Select(n => SyntaxFactory.UsingDirective(SyntaxFactory.ParseName(n)))));
                
                DocumentEditor.ReplaceNode(node, newcompilationUnitNode);
            }
        }
    }
}