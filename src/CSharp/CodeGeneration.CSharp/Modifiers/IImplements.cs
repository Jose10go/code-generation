﻿using CodeGen.Attributes;
using CodeGen.Context;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.Linq;
using static CodeGen.CSharp.Extensions;
namespace CodeGen.CSharp.Context
{
    public abstract partial class CSharpContext : CodeGenContext<Project, CSharpSyntaxNode,CompilationUnitSyntax, ISymbol>
    {
        [CommandModifier]
        public interface IImplements<TCommand>
            where TCommand:Core.ICommand
        {
            BaseListSyntax ImplementedInterfaces { get; set; }
            TCommand Implements(params string[] interfaces)
            {
                if (ImplementedInterfaces is null)
                    ImplementedInterfaces = SyntaxFactory.BaseList();
                ImplementedInterfaces=ImplementedInterfaces.AddTypes(
                    interfaces.Select(
                        name => SyntaxFactory.SimpleBaseType(
                            SyntaxFactory.ParseTypeName(name))).ToArray());
                return (TCommand)this;
            }

            TCommand Implements<T>()
            {
                return this.Implements(GetCSharpName<T>());
            }

            TCommand Implements<T1,T2>()
            {
                return this.Implements(GetCSharpName<T1>(), 
                                       GetCSharpName<T2>());
            }

            TCommand Implements<T1, T2, T3>()
            {
                return this.Implements(GetCSharpName<T1>(),
                                       GetCSharpName<T2>(),
                                       GetCSharpName<T3>());
            }

            TCommand Implements<T1, T2, T3, T4>()
            {
                return this.Implements(GetCSharpName<T1>(),
                                       GetCSharpName<T2>(),
                                       GetCSharpName<T3>(),
                                       GetCSharpName<T4>());
            }

            TCommand Implements<T1, T2, T3, T4, T5>()
            {
                return this.Implements(GetCSharpName<T1>(),
                                       GetCSharpName<T2>(),
                                       GetCSharpName<T3>(),
                                       GetCSharpName<T4>(),
                                       GetCSharpName<T5>());
            }
        }
    }
}
