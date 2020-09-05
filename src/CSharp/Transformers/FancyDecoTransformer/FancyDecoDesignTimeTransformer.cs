using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis;
using CodeGeneration.CSharp;
using static CodeGen.CSharp.Context.CSharpContext;
using CodeGen.CSharp;
using FancyDecoCore;
using System;
using System.IO;

namespace FancyDecoTransformer
{
    public class FancyDecoDesignTimeTransformer : CodeGenerationDesignTimeTransformer
    {
        public override void Transform()
        {
            var decoratorsDefinitions =
               Engine.Select<ClassDeclarationSyntax, NamespaceDeclarationSyntax>()
                     .Where(target => IsFancyDecoratorDefinition(target.SemanticSymbol as INamedTypeSymbol));

            foreach (var target in decoratorsDefinitions)
            {
                var att = Engine.SelectNew(GetAtributeNewPath(target.DocumentPath))
                                .Execute((ICreateNamespace cmd)=>
                                     cmd.WithName(target.Parent.Node.Name.ToString())
                                        .Using("System")
                                        .Using<DecoratorAttribute>())
                                .Execute((ICreateClass cmd) =>
                                     cmd.MakePublic()
                                        .InheritsFrom<DecoratorAttribute>()
                                        .WithName(target.Node.Identifier.ToString() + "Attribute"));

                //Generate TypeCheck Attributes
                //.WithAttributes(
                // (target.SemanticSymbol as ITypeSymbol).BaseType.Name switch
                // {
                //     "Decorator" => new string[0],
                //     _ => throw new Exception()
                // }));

                //TODO: make constructors....
                //foreach (var constructor in target.Select<ConstructorDeclarationSyntax>())
                //    constructor.Execute((ICloneConstructor cmd) =>
                //        cmd.WithBody(new CodeContext())
                //           .On(att.Node));
            }
        }

        private string GetAtributeNewPath(string documentPath)
        {
            return Path.GetFileNameWithoutExtension(documentPath) + "Attribute.cs";
        }

        private bool IsFancyDecoratorDefinition(INamedTypeSymbol symbol) 
        {
            return symbol.BaseType.Name == "Decorator";
        }

    }

}
