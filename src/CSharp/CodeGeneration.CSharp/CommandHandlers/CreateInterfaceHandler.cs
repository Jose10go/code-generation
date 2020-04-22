using CodeGen.Attributes;
using CodeGen.Context;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Editing;
using System;
using System.Linq;

namespace CodeGen.CSharp.Context
{
    public abstract partial class CSharpContext : CodeGenContext<Project, CSharpSyntaxNode, CompilationUnitSyntax, ISymbol>
    {
        [CommandHandler]
        public class CreateInterfaceCommandHandler : CommandHandler<ICreateInterface, NamespaceDeclarationSyntax, InterfaceDeclarationSyntax>
        {
            public CreateInterfaceCommandHandler(ICreateInterface command) : base(command)
            {
            }

            protected override InterfaceDeclarationSyntax ProccessNode(NamespaceDeclarationSyntax node, DocumentEditor documentEditor,Guid id)
            {
                var modifiers = new SyntaxTokenList();
                if (Command.Modifiers != default)
                    modifiers = modifiers.Add(Command.Modifiers);
                if (Command.Partial != default)
                    modifiers = modifiers.Add(Command.Partial);

                var separatedBaseTypes = new SeparatedSyntaxList<BaseTypeSyntax>();
                if (Command.ImplementedInterfaces != null)
                    separatedBaseTypes.AddRange(Command.ImplementedInterfaces.Select(name => SyntaxFactory.SimpleBaseType(SyntaxFactory.ParseTypeName(name))));

                var classNode = SyntaxFactory.InterfaceDeclaration(Command.Name)
                                             .WithAttributeLists(Command.Attributes)
                                             .WithModifiers(modifiers)
                                             .WithBaseList(SyntaxFactory.BaseList().WithTypes(separatedBaseTypes))
                                             .WithAdditionalAnnotations(new SyntaxAnnotation($"{id}"));

                documentEditor.InsertMembers(node, 0, new[] { classNode });
                return classNode;
            }

        }
    }
}