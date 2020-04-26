using CodeGen.Attributes;
using CodeGen.Context;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Editing;
using static CodeGen.CSharp.Extensions;
namespace CodeGen.CSharp.Context
{
    public abstract partial class CSharpContext : CodeGenContext<Project, CSharpSyntaxNode, CompilationUnitSyntax, ISymbol>
    {
        [CommandHandler]
        public class CloneStructCommandHandler : CommandHandler<ICloneStruct>
        {
            public CloneStructCommandHandler(ICloneStruct command) : base(command)
            {
            }

            public override void VisitStructDeclaration(StructDeclarationSyntax node)
            {
                var modifiers = GetModifiers(Command.Modifiers,Command.Partial);
                var baseTypes = GetBaseTypes(Command.ImplementedInterfaces);

                var structNode = node.WithIdentifier(SyntaxFactory.Identifier(Command.Name))
                                     .WithTypeParameterList(Command.GenericParameters)
                                     .WithConstraintClauses(Command.GenericParametersConstraints)
                                     .WithBaseList(baseTypes)
                                     .WithAttributeLists(Command.Attributes)
                                     .WithModifiers(modifiers)
                                     .WithAdditionalAnnotations(new SyntaxAnnotation($"{Id}"));

                
                DocumentEditor.InsertAfter(node,structNode);
            }

        }
    }
}