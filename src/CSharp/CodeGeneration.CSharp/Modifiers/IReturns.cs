using CodeGen.Attributes;
using CodeGen.Context;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using static CodeGen.CSharp.Extensions;
namespace CodeGen.CSharp.Context
{
    public abstract partial class CSharpContext : CodeGenContext<Project, CSharpSyntaxNode,CompilationUnitSyntax, ISymbol>
    {
        [CommandModifier]
        public interface IReturns<TCommand>
            where TCommand:Core.ICommand
        {
            TypeSyntax ReturnType { get; set; }
            TCommand Returns(string returnType) 
            {
                this.ReturnType = SyntaxFactory.ParseTypeName(returnType);
                return (TCommand)this;
            }

            TCommand Returns<T>()
            {
                return this.Returns(GetCSharpName<T>());
            }

        }
    }
}
