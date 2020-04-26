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
        public class ModifyClassCommandHandler :CommandHandler<IModifyClass> 
        {
            public ModifyClassCommandHandler(IModifyClass command) : base(command)
            {
            }

            public override void VisitClassDeclaration(ClassDeclarationSyntax node)
            {
                var modifiers = GetModifiers(Command.Modifiers, Command.Abstract, Command.Static, Command.Partial);
                var baseTypes = GetBaseTypes(Command.ImplementedInterfaces, Command.InheritsType);

                var classNode = node.WithIdentifier(SyntaxFactory.Identifier(Command.Name))
                                  .WithTypeParameterList(Command.GenericParameters)
                                  .WithConstraintClauses(Command.GenericParametersConstraints)
                                  .WithAttributeLists(Command.Attributes)
                                  .WithModifiers(modifiers)
                                  .WithBaseList(baseTypes)
                                  .WithAdditionalAnnotations(new SyntaxAnnotation($"{Id}"));

                DocumentEditor.ReplaceNode(node, classNode);
            }
        }
    }
}