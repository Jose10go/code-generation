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
                if (newGetAccessor.Body is null)
                    newGetAccessor = newGetAccessor.WithSemicolonToken(SyntaxFactory.Token(SyntaxKind.SemicolonToken));

                var newSetAccessor =SyntaxFactory.AccessorDeclaration(SyntaxKind.SetAccessorDeclaration)
                                                 .WithModifiers(Command.SetModifier)
                                                 .WithExpressionBody(Command.SetExpression)
                                                 .WithBody(Command.SetStatements);
               
                if (newSetAccessor.Body is null)
                    newSetAccessor = newSetAccessor.WithSemicolonToken(SyntaxFactory.Token(SyntaxKind.SemicolonToken));

                var Accesors = new SyntaxList<AccessorDeclarationSyntax>().Add(newGetAccessor).Add(newSetAccessor);

                var property = SyntaxFactory.PropertyDeclaration(Command.ReturnType??SyntaxFactory.ParseTypeName("object"), Command.Name)
                                            .WithAttributeLists(Command.Attributes)
                                            .WithModifiers(modifiers)
                                            .WithAdditionalAnnotations(new SyntaxAnnotation($"{Id}"))
                                            .WithAccessorList(SyntaxFactory.AccessorList(Accesors));

                if (Command.InitializerExpression != null)
                    property = property.WithInitializer(SyntaxFactory.EqualsValueClause(Command.InitializerExpression))
                                       .WithSemicolonToken(SyntaxFactory.Token(SyntaxKind.SemicolonToken));


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