using CodeGen.Attributes;
using CodeGen.Context;
using CodeGen.Core;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CodeGen.CSharp.Context
{
    public abstract partial class CSharpContext : CodeGenContext<Project, CSharpSyntaxNode, CompilationUnitSyntax, ISymbol>
    {
        [CommandModifier]
        public interface IGet<TCommand>
            where TCommand : Core.ICommand
        {
            
            public TCommand Get<T>(Key<T> key, out T value)
                where T:class
            {
                var self = (TCommand)this;
                self.SingleTarget.Get(key, out value);
                return self;
            }
        }
    }
}
