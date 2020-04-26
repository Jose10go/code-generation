using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Editing;
using CodeGen.Attributes;
using Microsoft.CodeAnalysis;
using CodeGen.Context;

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
                var modifiers = new SyntaxTokenList();
                if (Command.Modifiers != default)
                    modifiers = modifiers.Add(Command.Modifiers);
                if (Command.Abstract != default)
                    modifiers = modifiers.Add(Command.Abstract);
                if (Command.Static != default)
                    modifiers = modifiers.Add(Command.Static);

                var getAccesor = SyntaxFactory.AccessorDeclaration(SyntaxKind.GetAccessorDeclaration);
                if (Command.GetStatements != null)
                    getAccesor = getAccesor.WithBody(Command.GetStatements);
                else
                    getAccesor = getAccesor.WithSemicolonToken(SyntaxFactory.Token(SyntaxKind.SemicolonToken));

                if (Command.GetModifier != default)
                    getAccesor = getAccesor.WithModifiers(new SyntaxTokenList(Command.GetModifier));
                
                var setAccesor = SyntaxFactory.AccessorDeclaration(SyntaxKind.SetAccessorDeclaration);
                
                if (Command.SetStatements != null)
                    setAccesor = setAccesor.WithBody(Command.SetStatements);
                else
                    setAccesor = setAccesor.WithSemicolonToken(SyntaxFactory.Token(SyntaxKind.SemicolonToken));

                
                if (Command.SetModifier != default)
                    setAccesor = setAccesor.WithModifiers(new SyntaxTokenList(Command.SetModifier));

                var Accesors = new SyntaxList<AccessorDeclarationSyntax>().Add(getAccesor).Add(setAccesor);

                var property = SyntaxFactory.PropertyDeclaration(SyntaxFactory.ParseTypeName(Command.ReturnType ?? "object"), Command.Name)
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