using CodeGen.Attributes;
using CodeGen.Context;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.Linq;

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

            TCommand WithGetBody(CodeContext code) 
            {
                var body = code.GetCode();
                GetStatements = body as BlockSyntax;
                GetExpression = SyntaxFactory.ArrowExpressionClause(body as ExpressionSyntax);
                return (TCommand)this;
            }
            TCommand WithSetBody(CodeContext code)
            {
                var body = code.GetCode();
                SetStatements = body as BlockSyntax;
                SetExpression = SyntaxFactory.ArrowExpressionClause(body as ExpressionSyntax);
                return (TCommand)this;
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
