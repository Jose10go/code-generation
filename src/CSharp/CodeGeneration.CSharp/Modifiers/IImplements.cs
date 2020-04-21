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
        public interface IImplements<TCommand>
            where TCommand:Core.ICommand
        {
            string[]  ImplementedInterfaces { get; set; }
            TCommand Implements(params string[] interfaces)
            {
                this.ImplementedInterfaces = interfaces;
                return (TCommand)this;
            }

            TCommand Implements<T>()
            {
                return this.Implements(typeof(T).Name);
            }

            TCommand Implements<T1,T2>()
            {
                return this.Implements(typeof(T1).Name,
                                       typeof(T2).Name);
            }

            TCommand Implements<T1, T2, T3>()
            {
                return this.Implements(typeof(T1).Name,
                                       typeof(T2).Name,
                                       typeof(T3).Name);
            }

            TCommand Implements<T1, T2, T3,T4>()
            {
                return this.Implements(typeof(T1).Name,
                                       typeof(T2).Name,
                                       typeof(T3).Name,
                                       typeof(T4).Name);
            }

            TCommand Implements<T1, T2, T3, T4, T5>()
            {
                return this.Implements(typeof(T1).Name,
                                       typeof(T2).Name,
                                       typeof(T3).Name,
                                       typeof(T4).Name,
                                       typeof(T5).Name);
            }
        }
    }
}
