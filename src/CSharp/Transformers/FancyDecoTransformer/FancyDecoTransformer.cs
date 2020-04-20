using Microsoft.CodeAnalysis.CSharp.Syntax;
using System;
using Microsoft.CodeAnalysis;
using CodeGeneration.CSharp;
using static CodeGen.CSharp.Context.CSharpContext;

namespace FancyDecoTransformer
{
    public class FancyDecoTransformer : CodeGenerationTransformer
    {
        public override void Transform()
        {
            var decorators = Engine.Select<ClassDeclarationSyntax>()
                                   .Where(target => IsFancyDecorator(target.SemanticSymbol));

            foreach (var decorator in decorators)
            {
                var x = 5;
                for (int i = 0; i < x; i++)
                    decorator.Execute((ICreateMethod cmd) => cmd);
            }
             

        }

        private bool IsFancyDecorator(ISymbol symbol) 
        {
            var classSymbol = symbol as INamedTypeSymbol;
            return classSymbol.BaseType.Name == "FancyDecorator";
        }
    }

}
