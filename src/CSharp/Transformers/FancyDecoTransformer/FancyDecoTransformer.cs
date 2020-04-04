using CodeGen.CSharp.Context.DocumentEdit;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System;
using Microsoft.CodeAnalysis;
using CodeGeneration.CSharp;

namespace FancyDecoTransformer
{
    public class FancyDecoTransformer : CodeGenerationTransformer
    {
        public override void Transform()
        {
            var decorators = Engine.Select<ClassDeclarationSyntax>()
                                   .Where((s, x) => IsFancyDecorator(s, x))
                                   .Execute<CSharpContextDocumentEditor.ICreateMethod>()
                                   .ForEach;

            Engine.ApplyChanges();
        }

        [A]
        [A,B,A]
        private bool IsFancyDecorator(ISymbol symbol, ClassDeclarationSyntax node) 
        {
            var classSymbol = symbol as INamedTypeSymbol;
            return classSymbol.BaseType.Name == "FancyDecorator";
        }
    }

    [AttributeUsage(AttributeTargets.Method,AllowMultiple =true)]
    public class A : Attribute 
    {
        public A()
        {
        }

        void f<T1, T2, T3, T4>(Func<T1,T2,T3,T4> a) 
        {
        }


    }

    [AttributeUsage(AttributeTargets.Method, AllowMultiple = true)]
    public class B : Attribute
    {
    }
}
