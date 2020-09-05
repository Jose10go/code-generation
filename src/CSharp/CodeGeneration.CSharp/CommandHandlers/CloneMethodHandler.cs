using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using CodeGen.Attributes;
using Microsoft.CodeAnalysis;
using CodeGen.Context;
using static CodeGen.CSharp.Extensions;
namespace CodeGen.CSharp.Context
{
    public abstract partial class CSharpContext : CodeGenContext<Project, CSharpSyntaxNode, CompilationUnitSyntax, ISymbol>
    {
        [CommandHandler]
        public class CloneMethodCommandHandler : CommandHandler<ICloneMethod>
        {
            public CloneMethodCommandHandler(ICloneMethod command) :base(command)
            {
            }
            
            public override void VisitMethodDeclaration(MethodDeclarationSyntax node)
            {
                var modifiers = GetModifiers(Command.Modifiers,Command.Abstract,Command.Static, Command.Partial);

                var methodNode = node.WithIdentifier(SyntaxFactory.ParseToken(Command.Name ?? node.Identifier.ToString()))
                                     .WithAttributeLists(Command.Attributes.Count > 0 ? Command.Attributes : node.AttributeLists)
                                     .WithParameterList(Command.Parameters ?? node.ParameterList)
                                     .WithTypeParameterList(Command.GenericParameters ?? node.TypeParameterList)
                                     .WithConstraintClauses(Command.GenericParametersConstraints.Count > 0 ? Command.GenericParametersConstraints : node.ConstraintClauses)
                                     .WithModifiers(modifiers.Count > 0 ? modifiers : node.Modifiers)
                                     .WithReturnType(Command.ReturnType ?? node.ReturnType)
                                     .WithBody(Command.BlockBody ?? (Command.ExpressionBody is null ? node.Body: null))
                                     .WithExpressionBody(Command.ExpressionBody ?? (Command.BlockBody is null ? node.ExpressionBody : null))
                                     .WithAdditionalAnnotations(new SyntaxAnnotation($"{Id}"));
                if (Command.ExpressionBody != null)
                    methodNode = methodNode.WithSemicolonToken(SyntaxFactory.Token(SyntaxKind.SemicolonToken));

                DocumentEditor.InsertAfter(node,methodNode);
            }
        }
    }
}