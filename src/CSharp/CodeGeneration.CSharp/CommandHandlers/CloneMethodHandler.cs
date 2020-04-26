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

                var methodNode = node.WithIdentifier(SyntaxFactory.ParseToken(Command.Name))
                                               .WithAttributeLists(Command.Attributes)
                                               .WithBody(Command.Body)
                                               .WithModifiers(modifiers)
                                               .WithAdditionalAnnotations(new SyntaxAnnotation($"{Id}"))
                                               .WithReturnType(Command.ReturnType ?? node.ReturnType);
                DocumentEditor.InsertAfter(node,methodNode);
            }
        }
    }
}