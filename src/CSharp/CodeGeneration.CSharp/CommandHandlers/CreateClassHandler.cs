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
        public class CreateClassCommandHandler : CommandHandler<ICreateClass>
        {
            public CreateClassCommandHandler(ICreateClass command) : base(command)
            {
            }

            private void ProccessNode(CSharpSyntaxNode node)
            {
                var modifiers = GetModifiers(Command.Modifiers, Command.Abstract, Command.Static, Command.Partial);
                var baseTypes = GetBaseTypes(Command.ImplementedInterfaces, Command.InheritsType);

                var classNode = SyntaxFactory.ClassDeclaration(Command.Name)
                                             .WithBaseList(baseTypes)
                                             .WithTypeParameterList(Command.GenericParameters)
                                             .WithConstraintClauses(Command.GenericParametersConstraints)
                                             .WithAttributeLists(Command.Attributes)
                                             .WithModifiers(modifiers)
                                             .WithAdditionalAnnotations(new SyntaxAnnotation($"{Id}"));
               
                DocumentEditor.InsertMembers(node, 0, new[] { classNode });
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