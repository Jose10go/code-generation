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
    public abstract partial class CSharpContext : CodeGenContext<Project, CSharpSyntaxNode,CompilationUnitSyntax,ISymbol>
    {
        [CommandModifier]
        public interface IWithAttribute<TCommandBuilder>
            where TCommandBuilder:Core.ICommand
        {
            SyntaxList<AttributeListSyntax> Attributes{ get; set; }
            TCommandBuilder WithAttributes(IEnumerable<string> Attributes) 
            {
                var attrs = Attributes.Select(x => SyntaxFactory.Attribute(SyntaxFactory.ParseName(x)));
                this.Attributes = new SyntaxList<AttributeListSyntax>(SyntaxFactory.AttributeList().AddAttributes(attrs.ToArray()));
                return (TCommandBuilder)this;
            }

            TCommandBuilder WithAttributes(params string[] Attributes)
            {
                return this.WithAttributes(Attributes.AsEnumerable());
            }

            TCommandBuilder WithAttributes<T>()
                where T:Attribute
            {
                return this.WithAttributes(typeof(T).Name);
            }

            TCommandBuilder WithAttributes<T1,T2>()
                where T1 : Attribute
                where T2 : Attribute
            {
                return this.WithAttributes(typeof(T1).Name,
                                           typeof(T2).Name);
            }

            TCommandBuilder WithAttributes<T1, T2, T3>()
                where T1 : Attribute
                where T2 : Attribute
                where T3 : Attribute
            {
                return this.WithAttributes(typeof(T1).Name, 
                                           typeof(T2).Name,
                                           typeof(T3).Name);
            }

            TCommandBuilder WithAttributes<T1, T2, T3, T4>()
                where T1 : Attribute
                where T2 : Attribute
                where T3 : Attribute
                where T4 : Attribute
            {
                return this.WithAttributes(typeof(T1).Name,
                                           typeof(T2).Name,
                                           typeof(T3).Name,
                                           typeof(T4).Name);
            }

            TCommandBuilder WithAttributes<T1, T2, T3, T4, T5>()
               where T1 : Attribute
               where T2 : Attribute
               where T3 : Attribute
               where T4 : Attribute
               where T5 : Attribute
            {
                return this.WithAttributes(typeof(T1).Name,
                                           typeof(T2).Name,
                                           typeof(T3).Name,
                                           typeof(T4).Name,
                                           typeof(T5).Name);
            }
        }
    }
}
