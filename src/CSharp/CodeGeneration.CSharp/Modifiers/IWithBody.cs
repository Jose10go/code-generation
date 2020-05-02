using CodeGen.Attributes;
using CodeGen.Context;
using CodeGen.Core.Exceptions;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System;
using System.Collections.Generic;

namespace CodeGen.CSharp.Context
{
    public abstract partial class CSharpContext : CodeGenContext<Project, CSharpSyntaxNode,CompilationUnitSyntax, ISymbol>
    {
        [CommandModifier]
        public interface IWithBody<TCommand>
            where TCommand:Core.ICommand,IWithParameters<TCommand>
        {
            BlockSyntax BlockBody { get; set; }
            ArrowExpressionClauseSyntax ExpressionBody { get; set; }
            TCommand WithBody(string body) 
            {
                this.BlockBody = SyntaxFactory.ParseStatement(body) as BlockSyntax;
                if (this.BlockBody != null)
                    this.ExpressionBody = null;
                else
                    this.ExpressionBody = SyntaxFactory.ArrowExpressionClause(SyntaxFactory.ParseExpression(body));
                return (TCommand)this;
            }

            TCommand WithBody(BlockSyntax body)
            {
                this.BlockBody = body;
                this.ExpressionBody = null;
                return (TCommand)this;
            }
            
            TCommand WithBody(ArrowExpressionClauseSyntax body)
            {
                this.BlockBody = null;
                this.ExpressionBody = body;
                return (TCommand)this;
            }

            TCommand WithBody(ParenthesizedLambdaExpressionSyntax codeContext,Dictionary<string, string> dynamicContext = null)
            {
                var substitutions = new Dictionary<string, Substitution>();
                if (dynamicContext != null)
                    foreach (var item in dynamicContext)
                        substitutions.Add(item.Key,new Substitution(item.Value,SubstitutionKind.DynamicMember));

                var command = (TCommand)this;
                substitutions.Add("this",new Substitution(codeContext.ParameterList.Parameters.First().Identifier.ToString(),SubstitutionKind.This));
                for (int i = 1; i < codeContext.ParameterList.Parameters.Count; i++)
                    substitutions.Add(codeContext.ParameterList.Parameters[i].ToString(),
                                      new Substitution(i-1< command.Parameters.Parameters.Count ? command.Parameters.Parameters[i-1].ToString() :"", SubstitutionKind.Parameter));

                var body = new SubstitutionContext(codeContext, substitutions).GetReplacedBody();
                BlockBody = body as BlockSyntax;
                ExpressionBody = body as ArrowExpressionClauseSyntax;
                return command;
            }

            TCommand WithBody<This>(Action<This> body, Dictionary<string, string> dynamicContext = null)
            {
                throw new NonIntendedException();
            }
            TCommand WithBody<This, TResult>(Func<This,TResult> body,Dictionary<string,string> dynamicContext=null)
            {
                throw new NonIntendedException();
            }
            TCommand WithBody<This, T1>(Action<This,T1> body,Dictionary<string,string> dynamicContext=null)
            {
                throw new NonIntendedException();
            }
            TCommand WithBody<This, T1,TResult>(Func<This,T1,TResult> body,Dictionary<string,string> dynamicContext=null)
            {
                throw new NonIntendedException();
            }
            TCommand WithBody<This, T1, T2>(Action<This, T1,T2> body,Dictionary<string,string> dynamicContext=null)
            {
                throw new NonIntendedException();
            }
            TCommand WithBody<This, T1, T2, TResult>(Func<This, T1,T2, TResult> body,Dictionary<string,string> dynamicContext=null)
            {
                throw new NonIntendedException();
            }
            TCommand WithBody<This, T1, T2, T3>(Action<This, T1, T2,T3> body,Dictionary<string,string> dynamicContext=null)
            {
                throw new NonIntendedException();
            }
            TCommand WithBody<This, T1, T2, T3, TResult>(Func<This, T1, T2,T3, TResult> body,Dictionary<string,string> dynamicContext=null)
            {
                throw new NonIntendedException();
            }
            TCommand WithBody<This, T1, T2, T3, T4>(Action<This, T1, T2, T3,T4> body,Dictionary<string,string> dynamicContext=null)
            {
                throw new NonIntendedException();
            }
            TCommand WithBody<This, T1, T2, T3, T4, TResult>(Func<This, T1, T2, T3,T4,TResult> body,Dictionary<string,string> dynamicContext=null)
            {
                throw new NonIntendedException();
            }
            TCommand WithBody<This, T1, T2, T3, T4, T5>(Action<This, T1, T2, T3, T4,T5> body,Dictionary<string,string> dynamicContext=null)
            {
                throw new NonIntendedException();
            }
            TCommand WithBody<This, T1, T2, T3, T4, T5, TResult>(Func<This, T1, T2, T3, T4,T5, TResult> body,Dictionary<string,string> dynamicContext=null)
            {
                throw new NonIntendedException();
            }
            TCommand WithBody<This, T1, T2, T3, T4, T5, T6>(Action<This, T1, T2, T3, T4, T5,T6> body,Dictionary<string,string> dynamicContext=null)
            {
                throw new NonIntendedException();
            }
            TCommand WithBody<This, T1, T2, T3, T4, T5, T6, TResult>(Func<This, T1, T2, T3, T4, T5,T6, TResult> body,Dictionary<string,string> dynamicContext=null)
            {
                throw new NonIntendedException();
            }
            TCommand WithBody<This, T1, T2, T3, T4, T5, T6, T7>(Action<This, T1, T2, T3, T4, T5, T6,T7> body,Dictionary<string,string> dynamicContext=null)
            {
                throw new NonIntendedException();
            }
            TCommand WithBody<This, T1, T2, T3, T4, T5, T6, T7, TResult>(Func<This, T1, T2, T3, T4, T5, T6,T7, TResult> body,Dictionary<string,string> dynamicContext=null)
            {
                throw new NonIntendedException();
            }
            TCommand WithBody<This, T1, T2, T3, T4, T5, T6, T7, T8>(Action<This, T1, T2, T3, T4, T5, T6,T7,T8> body,Dictionary<string,string> dynamicContext=null)
            {
                throw new NonIntendedException();
            }
            TCommand WithBody<This, T1, T2, T3, T4, T5, T6, T7, T8, TResult>(Func<This, T1, T2, T3, T4, T5, T6,T7,T8, TResult> body,Dictionary<string,string> dynamicContext=null)
            {
                throw new NonIntendedException();
            }
            TCommand WithBody<This, T1, T2, T3, T4, T5, T6, T7, T8, T9>(Action<This, T1, T2, T3, T4, T5, T6,T7,T8,T9> body,Dictionary<string,string> dynamicContext=null)
            {
                throw new NonIntendedException();
            }
            TCommand WithBody<This, T1, T2, T3, T4, T5, T6, T7, T8, T9, TResult>(Func<This, T1, T2, T3, T4, T5, T6,T7,T8,T9, TResult> body,Dictionary<string,string> dynamicContext=null)
            {
                throw new NonIntendedException();
            }
            TCommand WithBody<This, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>(Action<This, T1, T2, T3, T4, T5, T6, T7, T8, T9,T10> body,Dictionary<string,string> dynamicContext=null)
            {
                throw new NonIntendedException();
            }
            TCommand WithBody<This, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, TResult>(Func<This, T1, T2, T3, T4, T5, T6, T7, T8, T9,T10, TResult> body,Dictionary<string,string> dynamicContext=null)
            {
                throw new NonIntendedException();
            }

        }
    }
}
