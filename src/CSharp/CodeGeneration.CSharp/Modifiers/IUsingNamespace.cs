using CodeGen.Attributes;
using CodeGen.Context;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.Collections.Generic;

namespace CodeGen.CSharp.Context
{
    public abstract partial class CSharpContext : CodeGenContext<Project, CSharpSyntaxNode,CompilationUnitSyntax, ISymbol>
    {
        [CommandModifier]
        public interface IUsingNamespace<TCommand>
            where TCommand:Core.ICommand
        {
            List<string> Namespaces { get; set; }
            TCommand Using(params string[] namespaces)
            {
                if (this.Namespaces is null)
                    this.Namespaces = new List<string>();
                this.Namespaces.AddRange(namespaces);
                return (TCommand)this;
            }

            TCommand Using<T>()
            {
                return this.Using(typeof(T).Namespace);
            }

            TCommand Using<T1,T2>()
            {
                return this.Using(typeof(T1).Namespace,
                                       typeof(T2).Namespace);
            }

            TCommand Using<T1, T2, T3>()
            {
                return this.Using(typeof(T1).Namespace,
                                       typeof(T2).Namespace,
                                       typeof(T3).Namespace);
            }

            TCommand Using<T1, T2, T3,T4>()
            {
                return this.Using(typeof(T1).Namespace,
                                       typeof(T2).Namespace,
                                       typeof(T3).Namespace,
                                       typeof(T4).Namespace);
            }

            TCommand Using<T1, T2, T3, T4, T5>()
            {
                return this.Using(typeof(T1).Namespace,
                                       typeof(T2).Namespace,
                                       typeof(T3).Namespace,
                                       typeof(T4).Namespace,
                                       typeof(T5).Namespace);
            }
        }
    }
}
