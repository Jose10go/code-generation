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
        public class CreateConstructorCommandHandler : CommandHandler<ICreateConstructor>
        {
            public CreateConstructorCommandHandler(ICreateConstructor command) :base(command)
            {
            }

            private void ProccessNode(CSharpSyntaxNode node)
            {
                var modifiers = GetModifiers(Command.Modifiers, Command.Static);

                var Constructor = SyntaxFactory.ConstructorDeclaration((node as TypeDeclarationSyntax).Identifier)
                                               .WithAttributeLists(Command.Attributes)
                                               .WithBody(Command.BlockBody)
                                               .WithExpressionBody(Command.ExpressionBody)
                                               .WithParameterList(Command.Parameters??SyntaxFactory.ParameterList())
                                               .WithModifiers(modifiers)
                                               .WithAdditionalAnnotations(new SyntaxAnnotation($"{Id}"));
                
                DocumentEditor.InsertMembers(node,0,new[]{Constructor});
            }

            public override void VisitClassDeclaration(ClassDeclarationSyntax node)
            {
                this.ProccessNode(node);
            }

            public override void VisitStructDeclaration(StructDeclarationSyntax node)
            {
                this.ProccessNode(node);
            }

        }
    }
}