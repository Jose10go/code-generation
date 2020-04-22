using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Editing;
using CodeGen.Attributes;
using Microsoft.CodeAnalysis;
using System;
using CodeGen.Context;

namespace CodeGen.CSharp.Context
{
    public abstract partial class CSharpContext:CodeGenContext<Project,CSharpSyntaxNode,CompilationUnitSyntax,ISymbol>
    {
        [CommandHandler]
        public class CreatePropertyCommandHandler : CommandHandler<ICreateProperty,ClassDeclarationSyntax,PropertyDeclarationSyntax>
        {
            public CreatePropertyCommandHandler(ICreateProperty command) :base(command)
            {
            }

            protected override PropertyDeclarationSyntax ProccessNode(ClassDeclarationSyntax node, DocumentEditor documentEditor,Guid id)
            {
                var modifiers = new SyntaxTokenList();
                if (Command.Modifiers != default)
                    modifiers = modifiers.Add(Command.Modifiers);
                if (Command.Abstract != default)
                    modifiers = modifiers.Add(Command.Abstract);
                if (Command.Static != default)
                    modifiers = modifiers.Add(Command.Static);
                var method = SyntaxFactory.PropertyDeclaration(SyntaxFactory.ParseTypeName("object"), Command.Name)
                                          .WithAttributeLists(Command.Attributes)
                                          .WithModifiers(modifiers)
                                          .WithAdditionalAnnotations(new SyntaxAnnotation($"{id}"));
                documentEditor.InsertMembers(node,0,new[]{method});
                return method;
            }
        }
    }
}