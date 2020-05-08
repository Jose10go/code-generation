using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using CodeGen.Attributes;
using Microsoft.CodeAnalysis;
using CodeGen.Context;
using static CodeGen.CSharp.Extensions;
using Microsoft.CodeAnalysis.Editing;

namespace CodeGen.CSharp.Context
{
    public abstract partial class CSharpContext : CodeGenContext<Project, CSharpSyntaxNode, CompilationUnitSyntax, ISymbol>
    {
        [CommandHandler]
        public class CloneConstructorCommandHandler : CommandHandler<ICloneConstructor>
        {
            public CloneConstructorCommandHandler(ICloneConstructor command) :base(command)
            {
            }

            public override void VisitConstructorDeclaration(ConstructorDeclarationSyntax node)
            {
                var modifiers = GetModifiers(Command.Modifiers,Command.Static);

                var ConstructorNode = node.WithIdentifier((Command.OnNode as TypeDeclarationSyntax)?.Identifier??node.Identifier)
                                          .WithAttributeLists(Command.Attributes.Count > 0 ? Command.Attributes : node.AttributeLists)
                                          .WithParameterList(Command.Parameters??node.ParameterList)
                                          .WithBody(Command.BlockBody ?? node.Body)
                                          .WithModifiers(modifiers.Count > 0 ? modifiers : node.Modifiers)
                                          .WithAdditionalAnnotations(new SyntaxAnnotation($"{Id}"));

                if (Command.OnNode is null)
                    DocumentEditor.InsertAfter(node, ConstructorNode);
                else
                    DocumentEditor.InsertMembers(Command.OnNode,0,new[] { ConstructorNode });
            }
        }
    }
}