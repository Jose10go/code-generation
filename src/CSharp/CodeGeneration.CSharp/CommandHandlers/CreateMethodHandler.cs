using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Editing;
using CodeGen.Attributes;
using Microsoft.CodeAnalysis;
using CodeGen.Context;
using static CodeGen.CSharp.Extensions;
using System;
using System.Linq;
using CodeGen.Core.Exceptions;

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

                var bodySyntax = Command.BlockBody;

                var method = SyntaxFactory.MethodDeclaration(Command.ReturnType??SyntaxFactory.ParseTypeName("void"), Command.Name)
                                          .WithAttributeLists(Command.Attributes)
                                          .WithBody(Command.BlockBody)
                                          .WithExpressionBody(Command.ExpressionBody)
                                          .WithParameterList(Command.Parameters??SyntaxFactory.ParameterList())
                                          .WithTypeParameterList(Command.GenericParameters)
                                          .WithConstraintClauses(Command.GenericParametersConstraints)
                                          .WithModifiers(modifiers)
                                          .WithAdditionalAnnotations(new SyntaxAnnotation($"{Id}"));
                
                if (Command.ExpressionBody != null)
                    method = method.WithSemicolonToken(SyntaxFactory.Token(SyntaxKind.SemicolonToken));
                
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