using CodeGen.Attributes;
using CodeGen.Context;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CodeGen.CSharp.Context
{
    public abstract partial class CSharpContext<TProcessEntity> : CodeGenContext<Project, CSharpSyntaxNode, CompilationUnitSyntax, ISymbol, TProcessEntity>
    {
        [CommandModifier]
        public interface IGet<TCommand,TNode>
            where TCommand : ICommand<TNode>
            where TNode:CSharpSyntaxNode
        {
            
            public TCommand Get<T>(Key<T> key, out T value)
                where T:class
            {
                var self = (TCommand)this;
                value = default;
                self.SingleTarget.Get(key, out value);
                return self;
            }
        }
    }
}
