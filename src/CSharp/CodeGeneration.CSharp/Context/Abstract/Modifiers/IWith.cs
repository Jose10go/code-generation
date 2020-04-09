using System;
using CodeGen.Attributes;
using CodeGen.Context;
using CodeGen.Core.Exceptions;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CodeGen.CSharp.Context
{
    public abstract partial class CSharpContext<TProcessEntity> : CodeGenContext<Project, CSharpSyntaxNode, CompilationUnitSyntax, ISymbol, TProcessEntity>
    {
        [CommandModifier]
        public interface IWith<TCommandBuilder>
            where TCommandBuilder:Core.ICommand
        {
            ExpressionSyntax NewExpression { get; set; }

            TCommandBuilder With(string exp)
            {
                this.NewExpression = SyntaxFactory.ParseExpression(exp);
                return (TCommandBuilder)this;
            }

            TCommandBuilder With(ExpressionSyntax exp)
            {
                this.NewExpression = exp;
                return (TCommandBuilder)this;
            }

            TCommandBuilder With<This>(Action<This> exp)
            {
                throw new NonIntendedException();
            }
            TCommandBuilder With<This, TResult>(Func<This,TResult> exp)
            {
                throw new NonIntendedException();
            }
            TCommandBuilder With<This, T1>(Action<This, T1> exp)
            {
                throw new NonIntendedException();
            }
            TCommandBuilder With<This, T1, TResult>(Func<This, T1, TResult> exp)
            {
                throw new NonIntendedException();
            }
            TCommandBuilder With<This, T1, T2>(Action<This, T1, T2> exp)
            {
                throw new NonIntendedException();
            }
            TCommandBuilder With<This, T1, T2, TResult>(Func<This, T1, T2, TResult> exp)
            {
                throw new NonIntendedException();
            }
            TCommandBuilder With<This, T1, T2, T3>(Action<This, T1, T2, T3> exp)
            {
                throw new NonIntendedException();
            }
            TCommandBuilder With<This, T1, T2, T3, TResult>(Func<This, T1, T2, T3, TResult> exp)
            {
                throw new NonIntendedException();
            }
            TCommandBuilder With<This, T1, T2, T3, T4>(Action<This, T1, T2, T3, T4> exp)
            {
                throw new NonIntendedException();
            }
            TCommandBuilder With<This, T1, T2, T3, T4, TResult>(Func<This, T1, T2, T3, T4, TResult> exp)
            {
                throw new NonIntendedException();
            }
            TCommandBuilder With<This, T1, T2, T3, T4, T5>(Action<This, T1, T2, T3, T4, T5> exp)
            {
                throw new NonIntendedException();
            }
            TCommandBuilder With<This, T1, T2, T3, T4, T5, TResult>(Func<This, T1, T2, T3, T4, T5, TResult> exp)
            {
                throw new NonIntendedException();
            }
            TCommandBuilder With<This, T1, T2, T3, T4, T5, T6>(Action<This, T1, T2, T3, T4, T5, T6> exp)
            {
                throw new NonIntendedException();
            }
            TCommandBuilder With<This, T1, T2, T3, T4, T5, T6, TResult>(Func<This, T1, T2, T3, T4, T5, T6, TResult> exp)
            {
                throw new NonIntendedException();
            }
            TCommandBuilder With<This, T1, T2, T3, T4, T5, T6, T7>(Action<This, T1, T2, T3, T4, T5, T6, T7> exp)
            {
                throw new NonIntendedException();
            }
            TCommandBuilder With<This, T1, T2, T3, T4, T5, T6, T7, TResult>(Func<This, T1, T2, T3, T4, T5, T6, T7, TResult> exp)
            {
                throw new NonIntendedException();
            }
            TCommandBuilder With<This, T1, T2, T3, T4, T5, T6, T7, T8>(Action<This, T1, T2, T3, T4, T5, T6, T7, T8> exp)
            {
                throw new NonIntendedException();
            }
            TCommandBuilder With<This, T1, T2, T3, T4, T5, T6, T7, T8, TResult>(Func<This, T1, T2, T3, T4, T5, T6, T7, T8, TResult> exp)
            {
                throw new NonIntendedException();
            }
            TCommandBuilder With<This, T1, T2, T3, T4, T5, T6, T7, T8, T9>(Action<This, T1, T2, T3, T4, T5, T6, T7, T8, T9> exp)
            {
                throw new NonIntendedException();
            }
            TCommandBuilder With<This, T1, T2, T3, T4, T5, T6, T7, T8, T9, TResult>(Func<This, T1, T2, T3, T4, T5, T6, T7, T8, T9, TResult> exp)
            {
                throw new NonIntendedException();
            }
            TCommandBuilder With<This, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>(Action<This, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10> exp)
            {
                throw new NonIntendedException();
            }
            TCommandBuilder With<This, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, TResult>(Func<This, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, TResult> exp)
            {
                throw new NonIntendedException();
            }

        }
    }
}
