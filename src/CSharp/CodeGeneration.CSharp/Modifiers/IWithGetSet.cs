using System;
using CodeGen.Attributes;
using CodeGen.Context;
using CodeGen.Core.Exceptions;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CodeGen.CSharp.Context
{
    public abstract partial class CSharpContext : CodeGenContext<Project, CSharpSyntaxNode, CompilationUnitSyntax, ISymbol>
    {
        [CommandModifier]
        public interface IWithGetSet<TCommandBuilder>
            where TCommandBuilder:Core.ICommand
        {
            SyntaxToken SetModifier { get; set; }
            SyntaxToken GetModifier { get; set; }
            BlockSyntax GetStatements { get; set; }
            BlockSyntax SetStatements { get; set; }

            TCommandBuilder WithGet(string exp)
            {
                this.GetStatements = SyntaxFactory.ParseStatement(exp) as BlockSyntax;
                return (TCommandBuilder)this;
            }

            TCommandBuilder WithGet(BlockSyntax exp)
            {
                this.GetStatements = exp;
                return (TCommandBuilder)this;
            }

            TCommandBuilder WithGet<ResultType>(Func<ResultType> exp)
            {
                throw new NonIntendedException();
            }

            TCommandBuilder WithSet(string exp)
            {
                this.SetStatements = SyntaxFactory.ParseStatement(exp) as BlockSyntax;
                return (TCommandBuilder)this;
            }

            TCommandBuilder WithSet(BlockSyntax exp)
            {
                this.SetStatements = exp;
                return (TCommandBuilder)this;
            }

            TCommandBuilder WithSet<ValueType>(Action<ValueType> exp)
            {
                throw new NonIntendedException();
            }

            TCommandBuilder MakeGetPublic()
            {
                GetModifier = SyntaxFactory.Token(SyntaxKind.PublicKeyword);
                return (TCommandBuilder)this;
            }
            TCommandBuilder MakeGetInternal()
            {
                GetModifier = SyntaxFactory.Token(SyntaxKind.InternalKeyword);
                return (TCommandBuilder)this;
            }
            TCommandBuilder MakeGetPrivate()
            {
                GetModifier = SyntaxFactory.Token(SyntaxKind.PrivateKeyword);
                return (TCommandBuilder)this;
            }
            TCommandBuilder MakeGetProtected()
            {
                GetModifier = SyntaxFactory.Token(SyntaxKind.ProtectedKeyword);
                return (TCommandBuilder)this;
            }

            TCommandBuilder MakeSetPublic()
            {
                SetModifier = SyntaxFactory.Token(SyntaxKind.PublicKeyword);
                return (TCommandBuilder)this;
            }
            TCommandBuilder MakeSetInternal()
            {
                SetModifier = SyntaxFactory.Token(SyntaxKind.InternalKeyword);
                return (TCommandBuilder)this;
            }
            TCommandBuilder MakeSetPrivate()
            {
                SetModifier = SyntaxFactory.Token(SyntaxKind.PrivateKeyword);
                return (TCommandBuilder)this;
            }
            TCommandBuilder MakeSetProtected()
            {
                SetModifier = SyntaxFactory.Token(SyntaxKind.ProtectedKeyword);
                return (TCommandBuilder)this;
            }
        }
    }
}
