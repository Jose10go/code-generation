﻿using CodeGen.Attributes;
using CodeGen.Context;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.Linq;

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
                var modifiers = new SyntaxTokenList();
                if (Command.Modifiers != default)
                    modifiers=modifiers.Add(Command.Modifiers);
                if (Command.Partial != default)
                    modifiers = modifiers.Add(Command.Partial);
                var separatedBaseTypes = new SeparatedSyntaxList<BaseTypeSyntax>();
                if (Command.ImplementedInterfaces != null)
                    separatedBaseTypes.AddRange(Command.ImplementedInterfaces.Select(name => SyntaxFactory.SimpleBaseType(SyntaxFactory.ParseTypeName(name))));

                var newNode = node.WithIdentifier(SyntaxFactory.Identifier(Command.Name))
                                  .WithAttributeLists(Command.Attributes)
                                  .WithModifiers(modifiers)
                                  .WithBaseList(SyntaxFactory.BaseList().WithTypes(separatedBaseTypes))
                                  .WithAdditionalAnnotations(new SyntaxAnnotation($"{Id}"));
                DocumentEditor.InsertAfter(node,newNode);
            }
        }
    }
}