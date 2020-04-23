using CodeGen.Context;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System;

namespace CodeGen.CSharp.Context
{
    public abstract partial class CSharpContext : CodeGenContext<Project, CSharpSyntaxNode, CompilationUnitSyntax, ISymbol>
    {
        public interface IType
        {
            public string TypeName { get; }
        }

        public class Type<T> : IType
        {
            public string TypeName => typeof(T).Name;
        }

        public class GenericType : IType
        {
            private readonly Type type;
            private readonly string[] genericParameters;

            public GenericType(Type type, params string[] genericParameters)
            {
                this.type = type;
                this.genericParameters = genericParameters;
            }

            public string TypeName => type.Name.Replace($"`{genericParameters.Length}", "<" + String.Join(',', genericParameters) + ">");
        }
    }
}