using CodeGen.Attributes;
using CodeGen.Context;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.Collections.Generic;
using static CodeGen.CSharp.Extensions;
namespace CodeGen.CSharp.Context
{
    public abstract partial class CSharpContext : CodeGenContext<Project, CSharpSyntaxNode, CompilationUnitSyntax, ISymbol>
    {
        [CommandModifier]
        public interface IWithGeneric<TCommand>
            where TCommand : Core.ICommand
        {
            Dictionary<string, List<string>> GenericTypes { get; set; }

            TCommand MakeGenericIn(params string[] genericTypes)
            {
                if (this.GenericTypes is null)
                    this.GenericTypes = new Dictionary<string, List<string>>();

                foreach (var item in genericTypes)
                    if (!GenericTypes.ContainsKey(item))
                        GenericTypes.Add(item, new List<string>());
                return (TCommand)this;
            }

            TCommand MakeGenericIn<T>()
            {
                return MakeGenericIn(GetCSharpName<T>());
            }

            TCommand MakeGenericIn<T1,T2>()
            {
                return MakeGenericIn(GetCSharpName<T1>(),
                                     GetCSharpName<T2>());
            }

            TCommand MakeGenericIn<T1, T2, T3>()
            {
                return MakeGenericIn(GetCSharpName<T1>(),
                                     GetCSharpName<T2>(),
                                     GetCSharpName<T3>());
            }

            TCommand MakeGenericIn<T1, T2, T3,T4>()
            {
                return MakeGenericIn(GetCSharpName<T1>(),
                                     GetCSharpName<T2>(),
                                     GetCSharpName<T3>(),
                                     GetCSharpName<T4>());
            }

            TCommand MakeGenericIn<T1, T2, T3, T4, T5>()
            {
                return MakeGenericIn(GetCSharpName<T1>(),
                                     GetCSharpName<T2>(),
                                     GetCSharpName<T3>(),
                                     GetCSharpName<T4>(),
                                     GetCSharpName<T5>());
            }

            TCommand WithConstraints(string type, params string[] constraints)
            {
                GenericTypes[type].AddRange(constraints);
                return (TCommand)this;
            }

            TCommand WithConstraints<T>(params string[] constraints)
            {
                return WithConstraints(GetCSharpName<T>(), constraints);
            }

            TCommand WithClassConstraint(string type)
            {
                return this.WithConstraints(type,"class");
            }

            TCommand WithClassConstraint<T>()
            {
                return this.WithConstraints(GetCSharpName<T>(), "class");
            }

            TCommand WithStructConstraint(string type)
            {
                return this.WithConstraints(type, "struct");
            }

            TCommand WithStructConstraint<T>()
            {
                return this.WithConstraints(GetCSharpName<T>(), "struct");
            }

            TCommand WithNewConstraint(string type)
            {
                return this.WithConstraints(type, "new()");
            }

            TCommand WithNewConstraint<T>()
            {
                return this.WithConstraints(GetCSharpName<T>(), "new()");
            }

            TCommand WithNotNullConstraint(string type)
            {
                return this.WithConstraints(type, "notnull");
            }

            TCommand WithNotNullConstraint<T>()
            {
                return this.WithConstraints(GetCSharpName<T>(), "notnull");
            }

        }
        
    }
}
