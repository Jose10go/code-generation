using CodeGen.Attributes;
using CodeGen.Context;
using CodeGen.Core.Exceptions;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System;

namespace CodeGen.CSharp.Context
{
    public abstract partial class CSharpContext<TProcessEntity> : CodeGenContext<Project, CSharpSyntaxNode,CompilationUnitSyntax, ISymbol, TProcessEntity>
    {
        [CommandModifier]
        public interface IWithBody<TCommandBuilder,TNode>
            where TCommandBuilder:Core.ICommand
            where TNode:CSharpSyntaxNode                    
        {
            BlockSyntax Body { get; set; }
            TCommandBuilder WithBody(string body) 
            {
                this.Body = (BlockSyntax)SyntaxFactory.ParseStatement(body);
                return (TCommandBuilder)this;
            }

            TCommandBuilder WithBody<This>(Action<This> body)
            {
                throw new NonIntendedException();
            }
            TCommandBuilder WithBody<This, TResult>(Func<TResult> body)
            {
                throw new NonIntendedException();
            }
            TCommandBuilder WithBody<This, T1>(Action<This,T1> body)
            {
                throw new NonIntendedException();
            }
            TCommandBuilder WithBody<This, T1,TResult>(Func<This,T1,TResult> body)
            {
                throw new NonIntendedException();
            }
            TCommandBuilder WithBody<This, T1, T2>(Action<This, T1,T2> body)
            {
                throw new NonIntendedException();
            }
            TCommandBuilder WithBody<This, T1, T2, TResult>(Func<This, T1,T2, TResult> body)
            {
                throw new NonIntendedException();
            }
            TCommandBuilder WithBody<This, T1, T2, T3>(Action<This, T1, T2,T3> body)
            {
                throw new NonIntendedException();
            }
            TCommandBuilder WithBody<This, T1, T2, T3, TResult>(Func<This, T1, T2,T3, TResult> body)
            {
                throw new NonIntendedException();
            }
            TCommandBuilder WithBody<This, T1, T2, T3, T4>(Action<This, T1, T2, T3,T4> body)
            {
                throw new NonIntendedException();
            }
            TCommandBuilder WithBody<This, T1, T2, T3, T4, TResult>(Func<This, T1, T2, T3,T4,TResult> body)
            {
                throw new NonIntendedException();
            }
            TCommandBuilder WithBody<This, T1, T2, T3, T4, T5>(Action<This, T1, T2, T3, T4,T5> body)
            {
                throw new NonIntendedException();
            }
            TCommandBuilder WithBody<This, T1, T2, T3, T4, T5, TResult>(Func<This, T1, T2, T3, T4,T5, TResult> body)
            {
                throw new NonIntendedException();
            }
            TCommandBuilder WithBody<This, T1, T2, T3, T4, T5, T6>(Action<This, T1, T2, T3, T4, T5,T6> body)
            {
                throw new NonIntendedException();
            }
            TCommandBuilder WithBody<This, T1, T2, T3, T4, T5, T6, TResult>(Func<This, T1, T2, T3, T4, T5,T6, TResult> body)
            {
                throw new NonIntendedException();
            }
            TCommandBuilder WithBody<This, T1, T2, T3, T4, T5, T6, T7>(Action<This, T1, T2, T3, T4, T5, T6,T7> body)
            {
                throw new NonIntendedException();
            }
            TCommandBuilder WithBody<This, T1, T2, T3, T4, T5, T6, T7, TResult>(Func<This, T1, T2, T3, T4, T5, T6,T7, TResult> body)
            {
                throw new NonIntendedException();
            }
            TCommandBuilder WithBody<This, T1, T2, T3, T4, T5, T6, T7, T8>(Action<This, T1, T2, T3, T4, T5, T6,T7,T8> body)
            {
                throw new NonIntendedException();
            }
            TCommandBuilder WithBody<This, T1, T2, T3, T4, T5, T6, T7, T8, TResult>(Func<This, T1, T2, T3, T4, T5, T6,T7,T8, TResult> body)
            {
                throw new NonIntendedException();
            }
            TCommandBuilder WithBody<This, T1, T2, T3, T4, T5, T6, T7, T8, T9>(Action<This, T1, T2, T3, T4, T5, T6,T7,T8,T9> body)
            {
                throw new NonIntendedException();
            }
            TCommandBuilder WithBody<This, T1, T2, T3, T4, T5, T6, T7, T8, T9, TResult>(Func<This, T1, T2, T3, T4, T5, T6,T7,T8,T9, TResult> body)
            {
                throw new NonIntendedException();
            }
            TCommandBuilder WithBody<This, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>(Action<This, T1, T2, T3, T4, T5, T6, T7, T8, T9,T10> body)
            {
                throw new NonIntendedException();
            }
            TCommandBuilder WithBody<This, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, TResult>(Func<This, T1, T2, T3, T4, T5, T6, T7, T8, T9,T10, TResult> body)
            {
                throw new NonIntendedException();
            }

        }
    }
}
