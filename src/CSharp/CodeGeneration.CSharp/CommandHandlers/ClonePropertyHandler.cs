using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Editing;
using CodeGen.Attributes;
using Microsoft.CodeAnalysis;
using System;
using CodeGen.Context;

namespace CodeGen.CSharp.Context
{
    public abstract partial class CSharpContext : CodeGenContext<Project, CSharpSyntaxNode, CompilationUnitSyntax, ISymbol>
    {
        [CommandHandler]
        public class ClonePropertyCommandHandler : CommandHandler<ICloneProperty,PropertyDeclarationSyntax>
        {
            public ClonePropertyCommandHandler(ICloneProperty command) :base(command)
            {
            }

            protected override PropertyDeclarationSyntax ProccessNode(PropertyDeclarationSyntax node,DocumentEditor documentEditor,Guid id)
            {
                var modifiers = new SyntaxTokenList();
                if (Command.Modifiers != default)
                    modifiers = modifiers.Add(Command.Modifiers);
                if (Command.Abstract != default)
                    modifiers = modifiers.Add(Command.Abstract);
                if (Command.Static != default)
                    modifiers = modifiers.Add(Command.Static);
                var methodNode = node.WithIdentifier(SyntaxFactory.ParseToken(Command.Name))
                                               .WithAttributeLists(Command.Attributes)
                                               .WithModifiers(modifiers)
                                               .WithAdditionalAnnotations(new SyntaxAnnotation($"{id}"));

                documentEditor.InsertAfter(node,methodNode);
                return methodNode;
            }
        }
    }
}