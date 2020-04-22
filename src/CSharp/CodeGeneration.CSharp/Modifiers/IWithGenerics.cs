using CodeGen.Attributes;
using CodeGen.Context;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.Collections.Generic;
using System.Linq;

namespace CodeGen.CSharp.Context
{
    public abstract partial class CSharpContext : CodeGenContext<Project, CSharpSyntaxNode,CompilationUnitSyntax, ISymbol>
    {
        [CommandModifier]
        public interface IWithGeneric<TCommand>
            where TCommand:Core.ICommand
        {
            Dictionary<string,List<string>> GenericTypes { get; set; }
            
            TCommand MakeGenricIn(params string[] genericTypes)
            {
                if (this.GenericTypes is null)
                    this.GenericTypes = new Dictionary<string, List<string>>();

                foreach (var item in genericTypes)
                    if (!GenericTypes.ContainsKey(item))
                        GenericTypes.Add(item, new List<string>());
                return (TCommand)this;
            }

            TCommand WithConstraints(string type,params string[] constraints)
            {
                GenericTypes[type].AddRange(constraints);
                return (TCommand)this;
            }
            
            TCommand WithConstraints<T1>(string type, params string[] constraints)
            {
                return this.WithConstraints(type,constraints.Append(typeof(T1).Namespace)
                                                            .ToArray());
            }
            TCommand WithConstraints<T1,T2>(string type, params string[] constraints)
            {
                return this.WithConstraints(type, constraints.Append(typeof(T1).Namespace)
                                                             .Append(typeof(T2).Namespace)
                                                             .ToArray());
            }

            TCommand WithConstraints<T1,T2,T3>(string type, params string[] constraints)
            {
                return this.WithConstraints(type, constraints.Append(typeof(T1).Namespace)
                                                             .Append(typeof(T2).Namespace)
                                                             .Append(typeof(T3).Namespace)
                                                             .ToArray());
            }

            TCommand WithConstraints<T1,T2,T3,T4>(string type, params string[] constraints)
            {
                return this.WithConstraints(type, constraints.Append(typeof(T1).Namespace)
                                                             .Append(typeof(T2).Namespace)
                                                             .Append(typeof(T3).Namespace)
                                                             .Append(typeof(T4).Namespace)
                                                             .ToArray());
            }

            TCommand WithConstraints<T1, T2, T3, T4,T5>(string type, params string[] constraints)
            {
                return this.WithConstraints(type, constraints.Append(typeof(T1).Namespace)
                                                             .Append(typeof(T2).Namespace)
                                                             .Append(typeof(T3).Namespace)
                                                             .Append(typeof(T4).Namespace)
                                                             .Append(typeof(T5).Namespace)
                                                             .ToArray());
            }
        }
    }
}
