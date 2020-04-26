using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Editing;
using CodeGen.Attributes;
using Microsoft.CodeAnalysis;
using CodeGen.Context;
using static CodeGen.CSharp.Extensions;
namespace CodeGen.CSharp.Context
{
    public abstract partial class CSharpContext:CodeGenContext<Project,CSharpSyntaxNode,CompilationUnitSyntax,ISymbol>
    {
        [CommandHandler]
        public class CreateMethodCommandHandler : CommandHandler<ICreateMethod>
        {
            public CreateMethodCommandHandler(ICreateMethod command) :base(command)
            {
            }

            private void ProccessNode(CSharpSyntaxNode node)
            {
                var modifiers = GetModifiers(Command.Modifiers, Command.Abstract, Command.Static, Command.Partial);
                
                var method = SyntaxFactory.MethodDeclaration(SyntaxFactory.ParseTypeName(Command.ReturnType??"void"), Command.Name)
                                          .WithAttributeLists(Command.Attributes)
                                          .WithBody(Command.Body)
                                          .WithModifiers(modifiers)
                                          .WithAdditionalAnnotations(new SyntaxAnnotation($"{Id}"));
                
                DocumentEditor.InsertMembers(node,0,new[]{method});
            }

            public override void VisitClassDeclaration(ClassDeclarationSyntax node)
            {
                this.ProccessNode(node);
            }

            public override void VisitStructDeclaration(StructDeclarationSyntax node)
            {
                this.ProccessNode(node);
            }

            public override void VisitInterfaceDeclaration(InterfaceDeclarationSyntax node)
            {
                this.ProccessNode(node);
            }

            public override void VisitMethodDeclaration(MethodDeclarationSyntax node)
            {
                this.ProccessNode(node);
            }
        }
    }
}