using System;
using CodeGen.Attributes;
using CodeGen.Context;
using CodeGen.Core.Exceptions;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.Linq;
namespace CodeGen.CSharp.Context
{
    public abstract partial class CSharpContext : CodeGenContext<Project, CSharpSyntaxNode, CompilationUnitSyntax, ISymbol>
    {
        [CommandModifier]
        public interface IWithGetSet<TCommandBuilder>
            where TCommandBuilder:Core.ICommand
        {
            SyntaxTokenList GetModifier { get; set; }
            SyntaxTokenList SetModifier { get; set; }
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

            TCommandBuilder MakeSetPublic()
            {
                if(!SetModifier.Any(x=>x.Kind()==SyntaxKind.PublicKeyword))
                    SetModifier = SetModifier.Add(SyntaxFactory.Token(SyntaxKind.PublicKeyword));
                SetModifier = SetModifier.Remove(SyntaxFactory.Token(SyntaxKind.PrivateKeyword));
                SetModifier = SetModifier.Remove(SyntaxFactory.Token(SyntaxKind.ProtectedKeyword));
                SetModifier = SetModifier.Remove(SyntaxFactory.Token(SyntaxKind.InternalKeyword));
                return (TCommandBuilder)this;
            }
            TCommandBuilder MakeSetInternal()
            {
                if(!SetModifier.Any(x=>x.Kind()==SyntaxKind.InternalKeyword))
                    SetModifier = SetModifier.Add(SyntaxFactory.Token(SyntaxKind.InternalKeyword));
                SetModifier = SetModifier.Remove(SyntaxFactory.Token(SyntaxKind.PublicKeyword));
                SetModifier = SetModifier.Remove(SyntaxFactory.Token(SyntaxKind.PrivateKeyword));
                return (TCommandBuilder)this;
            }
            TCommandBuilder MakeSetPrivate()
            {
                if (!SetModifier.Any(x => x.Kind() == SyntaxKind.PrivateKeyword))
                    SetModifier = SetModifier.Add(SyntaxFactory.Token(SyntaxKind.PrivateKeyword));
                SetModifier = SetModifier.Remove(SyntaxFactory.Token(SyntaxKind.PublicKeyword));
                SetModifier = SetModifier.Remove(SyntaxFactory.Token(SyntaxKind.ProtectedKeyword));
                SetModifier = SetModifier.Remove(SyntaxFactory.Token(SyntaxKind.InternalKeyword));
                return (TCommandBuilder)this;
            }
            TCommandBuilder MakeSetProtected()
            {
                if (!SetModifier.Any(x => x.Kind() == SyntaxKind.ProtectedKeyword))
                    SetModifier = SetModifier.Add(SyntaxFactory.Token(SyntaxKind.ProtectedKeyword));
                SetModifier = SetModifier.Remove(SyntaxFactory.Token(SyntaxKind.PublicKeyword));
                SetModifier = SetModifier.Remove(SyntaxFactory.Token(SyntaxKind.PrivateKeyword));
                return (TCommandBuilder)this;
            }

            TCommandBuilder MakeGetPublic()
            {
                if (!GetModifier.Any(x => x.Kind() == SyntaxKind.PublicKeyword))
                    GetModifier = GetModifier.Add(SyntaxFactory.Token(SyntaxKind.PublicKeyword));
                GetModifier = GetModifier.Remove(SyntaxFactory.Token(SyntaxKind.PrivateKeyword));
                GetModifier = GetModifier.Remove(SyntaxFactory.Token(SyntaxKind.ProtectedKeyword));
                GetModifier = GetModifier.Remove(SyntaxFactory.Token(SyntaxKind.InternalKeyword));
                return (TCommandBuilder)this;
            }
            TCommandBuilder MakeGetInternal()
            {
                if (!GetModifier.Any(x => x.Kind() == SyntaxKind.InternalKeyword))
                    GetModifier = GetModifier.Add(SyntaxFactory.Token(SyntaxKind.InternalKeyword));
                GetModifier = GetModifier.Remove(SyntaxFactory.Token(SyntaxKind.PublicKeyword));
                GetModifier = GetModifier.Remove(SyntaxFactory.Token(SyntaxKind.PrivateKeyword));
                return (TCommandBuilder)this;
            }
            TCommandBuilder MakeGetPrivate()
            {
                if (!GetModifier.Any(x => x.Kind() == SyntaxKind.PrivateKeyword))
                    GetModifier = GetModifier.Add(SyntaxFactory.Token(SyntaxKind.PrivateKeyword));
                GetModifier = GetModifier.Remove(SyntaxFactory.Token(SyntaxKind.PublicKeyword));
                GetModifier = GetModifier.Remove(SyntaxFactory.Token(SyntaxKind.ProtectedKeyword));
                GetModifier = GetModifier.Remove(SyntaxFactory.Token(SyntaxKind.InternalKeyword));
                return (TCommandBuilder)this;
            }
            TCommandBuilder MakeGetProtected()
            {
                if (!GetModifier.Any(x => x.Kind() == SyntaxKind.ProtectedKeyword))
                    GetModifier = GetModifier.Add(SyntaxFactory.Token(SyntaxKind.ProtectedKeyword));
                GetModifier = GetModifier.Remove(SyntaxFactory.Token(SyntaxKind.PublicKeyword));
                GetModifier = GetModifier.Remove(SyntaxFactory.Token(SyntaxKind.PrivateKeyword));
                return (TCommandBuilder)this;
            }

        }
    }
}
