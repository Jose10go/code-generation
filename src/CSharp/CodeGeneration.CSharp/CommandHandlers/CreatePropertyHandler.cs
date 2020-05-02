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
        public class CreatePropertyCommandHandler : CommandHandler<ICreateProperty>
        {
            public CreatePropertyCommandHandler(ICreateProperty command) :base(command)
            {
            }

            private void ProccessNode(CSharpSyntaxNode node)
            {
                var modifiers = GetModifiers(Command.Modifiers,Command.Abstract,Command.Static);

                var newGetAccessor = SyntaxFactory.AccessorDeclaration(SyntaxKind.GetAccessorDeclaration)
                                                  .WithModifiers(Command.GetModifier)
                                                  .WithExpressionBody(Command.GetExpression)
                                                  .WithBody(Command.GetStatements);

                var newSetAccessor =SyntaxFactory.AccessorDeclaration(SyntaxKind.SetAccessorDeclaration)
                                                 .WithModifiers(Command.SetModifier)
                                                 .WithExpressionBody(Command.SetExpression)
                                                 .WithBody(Command.SetStatements);

                var Accesors = new SyntaxList<AccessorDeclarationSyntax>().Add(newGetAccessor).Add(newSetAccessor);

                var property = SyntaxFactory.PropertyDeclaration(Command.ReturnType??SyntaxFactory.ParseTypeName("object"), Command.Name)
                                            .WithAttributeLists(Command.Attributes)
                                            .WithModifiers(modifiers)
                                            .WithAdditionalAnnotations(new SyntaxAnnotation($"{Id}"))
                                            .WithAccessorList(SyntaxFactory.AccessorList(Accesors));
                
                DocumentEditor.InsertMembers(node,0,new[]{property});
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
        }
    }
}