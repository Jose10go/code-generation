using System;
using CodeGen.Attributes;
using CodeGen.Context;
using CodeGen.Core.Exceptions;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.Linq;
using System.Collections.Generic;

namespace CodeGen.CSharp.Context
{
    public abstract partial class CSharpContext : CodeGenContext<Project, CSharpSyntaxNode, CompilationUnitSyntax, ISymbol>
    {
        [CommandModifier]
        public interface IWithGetSet<TCommand>
            where TCommand:Core.ICommand
        {
            SyntaxTokenList GetModifier { get; set; }
            SyntaxTokenList SetModifier { get; set; }
            BlockSyntax GetStatements { get; set; }
            BlockSyntax SetStatements { get; set; }
            ArrowExpressionClauseSyntax GetExpression { get; set; }
            ArrowExpressionClauseSyntax SetExpression { get; set; }
            
            TCommand WithGet(string exp)
            {
                this.GetStatements = SyntaxFactory.ParseStatement(exp) as BlockSyntax;
                if (this.GetStatements != null)
                    this.GetExpression = null;
                else
                    this.GetExpression = SyntaxFactory.ArrowExpressionClause(SyntaxFactory.ParseExpression(exp));
                return (TCommand)this;
            }

            TCommand WithGet(BlockSyntax exp)
            {
                this.GetExpression = null;
                this.GetStatements = exp;
                return (TCommand)this;
            }

            TCommand WithGet(ArrowExpressionClauseSyntax exp)
            {
                this.GetStatements = null;
                this.GetExpression = exp;
                return (TCommand)this;
            }

            TCommand WithGet<This,ResultType>(Func<This,ResultType> exp, Dictionary<string, string> dynamicContext = null)
            {
                throw new NonIntendedException();
            }

            TCommand WithGet(ParenthesizedLambdaExpressionSyntax codeContext, Dictionary<string, string> dynamicContext = null)
            {
                var substitutions = new Dictionary<string, Substitution>();
                if (dynamicContext != null)
                    foreach (var item in dynamicContext)
                        substitutions.Add(item.Key, new Substitution(item.Value, SubstitutionKind.DynamicMember));

                var command = (TCommand)this;
                substitutions.Add(codeContext.ParameterList.Parameters.First().Identifier.ToString(), new Substitution("this", SubstitutionKind.This));

                var body = new SubstitutionContext(codeContext, substitutions).GetReplacedBody();
                GetStatements = body as BlockSyntax;
                GetExpression = body as ArrowExpressionClauseSyntax;
                return command;
            }

            TCommand WithSet(string exp)
            {
                this.SetStatements = SyntaxFactory.ParseStatement(exp) as BlockSyntax;
                if (this.SetStatements != null)
                    this.SetExpression = null;
                else
                    this.SetExpression = SyntaxFactory.ArrowExpressionClause(SyntaxFactory.ParseExpression(exp));
                return (TCommand)this;
            }

            TCommand WithSet(BlockSyntax exp)
            {
                this.SetStatements = exp;
                this.SetExpression = null;
                return (TCommand)this;
            }

            TCommand WithSet(ArrowExpressionClauseSyntax exp)
            {
                this.SetStatements = null;
                this.SetExpression = exp;
                return (TCommand)this;
            }

            TCommand WithSet<This,ValueType>(Action<This,ValueType> exp, Dictionary<string, string> dynamicContext = null)
            {
                throw new NonIntendedException();
            }

            TCommand WithSet(ParenthesizedLambdaExpressionSyntax codeContext, Dictionary<string, string> dynamicContext = null)
            {
                var substitutions = new Dictionary<string, Substitution>();
                if (dynamicContext != null)
                    foreach (var item in dynamicContext)
                        substitutions.Add(item.Key, new Substitution(item.Value, SubstitutionKind.DynamicMember));

                var command = (TCommand)this;
                substitutions.Add(codeContext.ParameterList.Parameters.First().Identifier.ToString(), new Substitution("this", SubstitutionKind.This));
                substitutions.Add(codeContext.ParameterList.Parameters.Skip(1).First().Identifier.ToString(), new Substitution("value", SubstitutionKind.Value));

                var body = new SubstitutionContext(codeContext, substitutions).GetReplacedBody();
                SetStatements = body as BlockSyntax;
                SetExpression = body as ArrowExpressionClauseSyntax;
                return command;
            }

            TCommand MakeSetPublic()
            {
                if(!SetModifier.Any(x=>x.Kind()==SyntaxKind.PublicKeyword))
                    SetModifier = SetModifier.Add(SyntaxFactory.Token(SyntaxKind.PublicKeyword));
                SetModifier = SetModifier.Remove(SyntaxFactory.Token(SyntaxKind.PrivateKeyword));
                SetModifier = SetModifier.Remove(SyntaxFactory.Token(SyntaxKind.ProtectedKeyword));
                SetModifier = SetModifier.Remove(SyntaxFactory.Token(SyntaxKind.InternalKeyword));
                return (TCommand)this;
            }
            TCommand MakeSetInternal()
            {
                if(!SetModifier.Any(x=>x.Kind()==SyntaxKind.InternalKeyword))
                    SetModifier = SetModifier.Add(SyntaxFactory.Token(SyntaxKind.InternalKeyword));
                SetModifier = SetModifier.Remove(SyntaxFactory.Token(SyntaxKind.PublicKeyword));
                SetModifier = SetModifier.Remove(SyntaxFactory.Token(SyntaxKind.PrivateKeyword));
                return (TCommand)this;
            }
            TCommand MakeSetPrivate()
            {
                if (!SetModifier.Any(x => x.Kind() == SyntaxKind.PrivateKeyword))
                    SetModifier = SetModifier.Add(SyntaxFactory.Token(SyntaxKind.PrivateKeyword));
                SetModifier = SetModifier.Remove(SyntaxFactory.Token(SyntaxKind.PublicKeyword));
                SetModifier = SetModifier.Remove(SyntaxFactory.Token(SyntaxKind.ProtectedKeyword));
                SetModifier = SetModifier.Remove(SyntaxFactory.Token(SyntaxKind.InternalKeyword));
                return (TCommand)this;
            }
            TCommand MakeSetProtected()
            {
                if (!SetModifier.Any(x => x.Kind() == SyntaxKind.ProtectedKeyword))
                    SetModifier = SetModifier.Add(SyntaxFactory.Token(SyntaxKind.ProtectedKeyword));
                SetModifier = SetModifier.Remove(SyntaxFactory.Token(SyntaxKind.PublicKeyword));
                SetModifier = SetModifier.Remove(SyntaxFactory.Token(SyntaxKind.PrivateKeyword));
                return (TCommand)this;
            }

            TCommand MakeGetPublic()
            {
                if (!GetModifier.Any(x => x.Kind() == SyntaxKind.PublicKeyword))
                    GetModifier = GetModifier.Add(SyntaxFactory.Token(SyntaxKind.PublicKeyword));
                GetModifier = GetModifier.Remove(SyntaxFactory.Token(SyntaxKind.PrivateKeyword));
                GetModifier = GetModifier.Remove(SyntaxFactory.Token(SyntaxKind.ProtectedKeyword));
                GetModifier = GetModifier.Remove(SyntaxFactory.Token(SyntaxKind.InternalKeyword));
                return (TCommand)this;
            }
            TCommand MakeGetInternal()
            {
                if (!GetModifier.Any(x => x.Kind() == SyntaxKind.InternalKeyword))
                    GetModifier = GetModifier.Add(SyntaxFactory.Token(SyntaxKind.InternalKeyword));
                GetModifier = GetModifier.Remove(SyntaxFactory.Token(SyntaxKind.PublicKeyword));
                GetModifier = GetModifier.Remove(SyntaxFactory.Token(SyntaxKind.PrivateKeyword));
                return (TCommand)this;
            }
            TCommand MakeGetPrivate()
            {
                if (!GetModifier.Any(x => x.Kind() == SyntaxKind.PrivateKeyword))
                    GetModifier = GetModifier.Add(SyntaxFactory.Token(SyntaxKind.PrivateKeyword));
                GetModifier = GetModifier.Remove(SyntaxFactory.Token(SyntaxKind.PublicKeyword));
                GetModifier = GetModifier.Remove(SyntaxFactory.Token(SyntaxKind.ProtectedKeyword));
                GetModifier = GetModifier.Remove(SyntaxFactory.Token(SyntaxKind.InternalKeyword));
                return (TCommand)this;
            }
            TCommand MakeGetProtected()
            {
                if (!GetModifier.Any(x => x.Kind() == SyntaxKind.ProtectedKeyword))
                    GetModifier = GetModifier.Add(SyntaxFactory.Token(SyntaxKind.ProtectedKeyword));
                GetModifier = GetModifier.Remove(SyntaxFactory.Token(SyntaxKind.PublicKeyword));
                GetModifier = GetModifier.Remove(SyntaxFactory.Token(SyntaxKind.PrivateKeyword));
                return (TCommand)this;
            }

        }
    }
}
