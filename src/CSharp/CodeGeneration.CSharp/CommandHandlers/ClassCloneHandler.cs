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
        public class ClassCloneCommandHandler :CommandHandler<IClassClone,ClassDeclarationSyntax> 
        {
            public ClassCloneCommandHandler(IClassClone command) : base(command)
            {
            }

            protected override ClassDeclarationSyntax ProccessNode(ClassDeclarationSyntax node, DocumentEditor documentEditor,Guid id)
            {
                var modifiers = new SyntaxTokenList();
                if (Command.Modifiers != default)
                    modifiers=modifiers.Add(Command.Modifiers);
                if (Command.Abstract != default)
                    modifiers = modifiers.Add(Command.Abstract);
                if (Command.Static != default)
                    modifiers = modifiers.Add(Command.Static);
                if (Command.Partial != default)
                    modifiers = modifiers.Add(Command.Partial);
                var newNode = node.WithIdentifier(SyntaxFactory.Identifier(Command.Name))
                                  .WithAttributeLists(Command.Attributes)
                                  .WithModifiers(modifiers)
                                  .WithAdditionalAnnotations(new SyntaxAnnotation($"{id}"));
                documentEditor.InsertAfter(node,newNode);
                return newNode;
            }
        }
    }
}