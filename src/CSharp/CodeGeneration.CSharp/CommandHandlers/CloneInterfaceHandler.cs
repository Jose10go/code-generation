using CodeGen.Attributes;
using CodeGen.Context;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.Linq;
using static CodeGen.CSharp.Extensions;
namespace CodeGen.CSharp.Context
{
    public abstract partial class CSharpContext : CodeGenContext<Project, CSharpSyntaxNode, CompilationUnitSyntax, ISymbol>
    {
        [CommandHandler]
        public class InterfaceCloneCommandHandler :CommandHandler<ICloneInterface> 
        {
            public InterfaceCloneCommandHandler(ICloneInterface command) : base(command)
            {
            }

            public override void VisitInterfaceDeclaration(InterfaceDeclarationSyntax node)
            {
                var modifiers = GetModifiers(Command.Modifiers, Command.Partial);
                var baseTypes = GetBaseTypes(Command.ImplementedInterfaces);

                var interfaceNode = node.WithIdentifier(SyntaxFactory.Identifier(Command.Name))
                                          .WithTypeParameterList(Command.GenericParameters)
                                          .WithConstraintClauses(Command.GenericParametersConstraints)
                                          .WithAttributeLists(Command.Attributes)
                                          .WithModifiers(modifiers)
                                          .WithBaseList(baseTypes)
                                          .WithAdditionalAnnotations(new SyntaxAnnotation($"{Id}"));
                                            

                DocumentEditor.InsertAfter(node,interfaceNode);
            }
        }
    }
}