using CodeGen.Attributes;
using CodeGen.Context;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Editing;
using System.Linq;
using static CodeGen.CSharp.Extensions;
namespace CodeGen.CSharp.Context
{
    public abstract partial class CSharpContext : CodeGenContext<Project, CSharpSyntaxNode, CompilationUnitSyntax, ISymbol>
    {
        [CommandHandler]
        public class CreateStructCommandHandler : CommandHandler<ICreateStruct>
        {
            public CreateStructCommandHandler(ICreateStruct command) : base(command)
            {
            }

            private void ProccessNode(CSharpSyntaxNode node)
            {
                var modifiers = GetModifiers(Command.Modifiers, Command.Partial);
                var baseTypes = GetBaseTypes(Command.ImplementedInterfaces);

                var structNode = SyntaxFactory.StructDeclaration(Command.Name)
                                             .WithBaseList(baseTypes)
                                             .WithTypeParameterList(Command.GenericParameters)
                                             .WithConstraintClauses(Command.GenericParametersConstraints)
                                             .WithAttributeLists(Command.Attributes)
                                             .WithModifiers(modifiers)
                                             .WithAdditionalAnnotations(new SyntaxAnnotation($"{Id}"));

                DocumentEditor.InsertMembers(node, 0, new[] { structNode });
            }

            public override void VisitNamespaceDeclaration(NamespaceDeclarationSyntax node)
            {
                this.ProccessNode(node);
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