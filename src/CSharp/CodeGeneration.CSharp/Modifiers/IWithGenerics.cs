using CodeGen.Attributes;
using CodeGen.Context;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System;
using System.Collections.Generic;
using System.Linq;

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

            TCommand WithConstraints(string type, params string[] constraints)
            {
                GenericTypes[type].AddRange(constraints);
                return (TCommand)this;
            }

            TCommand WithTypeConstraints(string type,params IType[] constraints)
            {
                return this.WithConstraints(type,constraints.Select(t=>t.TypeName).ToArray());
            }

            TCommand WithClassConstraint(string type)
            {
                return this.WithConstraints(type,"class");
            }

            TCommand WithStructConstraint(string type)
            {
                return this.WithConstraints(type, "struct");
            }

            TCommand WithNewConstraint(string type)
            {
                return this.WithConstraints(type, "new()");
            }

            TCommand WithNotNullConstraint(string type)
            {
                return this.WithConstraints(type, "notnull");
            }
            
        }
        
    }
}
