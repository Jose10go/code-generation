using CodeGen.Attributes;
using CodeGen.Context;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Editing;
using System;

namespace CodeGen.CSharp.Context
{
    public abstract partial class CSharpContext : CodeGenContext<Project, CSharpSyntaxNode, CompilationUnitSyntax, ISymbol>
    {
        [CommandHandler]
        public class CreateClassCommandHandler : CommandHandler<ICreateClass, NamespaceDeclarationSyntax, ClassDeclarationSyntax>
        {
            public CreateClassCommandHandler(ICreateClass command) : base(command)
            {
            }

            protected override ClassDeclarationSyntax ProccessNode(NamespaceDeclarationSyntax node, DocumentEditor documentEditor,Guid id)
            {
                var modifiers = new SyntaxTokenList();
                if (Command.Modifiers != default)
                    modifiers = modifiers.Add(Command.Modifiers);
                if (Command.Abstract != default)
                    modifiers = modifiers.Add(Command.Abstract);
                if (Command.Static != default)
                    modifiers = modifiers.Add(Command.Static);
                if (Command.Partial != default)
                    modifiers = modifiers.Add(Command.Partial);
                var classNode = SyntaxFactory.ClassDeclaration(Command.Name)
                                             .WithAttributeLists(Command.Attributes)
                                             .WithModifiers(modifiers)
                                             .WithAdditionalAnnotations(new SyntaxAnnotation($"{id}"));
                documentEditor.InsertMembers(node, 0, new[] { classNode });
                return classNode;
            }

        }
    }
}