using CodeGen.Attributes;
using CodeGen.Context;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
namespace CodeGen.CSharp.Context
{
    public abstract partial class CSharpContext : CodeGenContext<Project, CSharpSyntaxNode,CompilationUnitSyntax, ISymbol>
    {
        [CommandModifier]
        public interface IReturns<TCommand>
            where TCommand:Core.ICommand
        {
            string ReturnType { get; set; }
            TCommand Returns(string returnType) 
            {
                this.ReturnType = returnType;
                return (TCommand)this;
            }

            TCommand Returns(IType type)
            {
                return this.Returns(type.TypeName);
            }

        }
    }
}
