using CodeGen.Attributes;
using CodeGen.Context;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.Linq;

namespace CodeGen.CSharp.Context
{
    public abstract partial class CSharpContext : CodeGenContext<Project, CSharpSyntaxNode,CompilationUnitSyntax, ISymbol>
    {
        [CommandModifier]
        public interface IImplements<TCommand>
            where TCommand:Core.ICommand
        {
            string[]  ImplementedInterfaces { get; set; }
            TCommand Implements(params string[] interfaces)
            {
                this.ImplementedInterfaces = interfaces;
                return (TCommand)this;
            }

            TCommand Implements(params IType[] interfaces)
            {
                return this.Implements(interfaces.Select(t =>t.TypeName).ToArray());
            }
        }
    }
}
