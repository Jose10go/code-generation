using CodeGen.Core.Exceptions;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CodeGen.CSharp
{
    public abstract class BaseCodeContext 
    {
        protected IEnumerable<StatementSyntax> statements;
        internal readonly Dictionary<string, CSharpSyntaxNode> idSubstitutions;
        
        protected BaseCodeContext()
        {
            idSubstitutions = new Dictionary<string, CSharpSyntaxNode>();
            statements = Enumerable.Empty<StatementSyntax>();
        }

        public CSharpSyntaxNode GetCode()
        {
            statements = statements.Select(
                x => x.ReplaceNodes(x.DescendantNodes()
                                     .OfType<IdentifierNameSyntax>()
                                     .Where(x => idSubstitutions.ContainsKey(x.Identifier.ToString())),
                                (node, _) =>idSubstitutions[node.Identifier.ToString()]));
            return SyntaxFactory.Block(statements);
        }
    }

    public sealed class CodeContext : BaseCodeContext
    {
        public Injector<CodeContext> InjectCode<T>(T value,out T _code)
            where T:CSharpSyntaxNode 
        {
            _code = value;
            return new Injector<CodeContext>(this,value);
        }
        public Injector<CodeContext> InjectThis<T>(out T @this)
        {
            @this = default;
            return new Injector<CodeContext>(this,SyntaxFactory.ThisExpression());
        }
        public Injector<CodeContext> InjectBase<T>(out T @base)
        {
            @base = default;
            return new Injector<CodeContext>(this, SyntaxFactory.BaseExpression());
        }
        public Injector<CodeContext> InjectType<T>() 
        {
            return new Injector<CodeContext>(this,SyntaxFactory.ParseTypeName(Extensions.GetCSharpName<T>()));
        }
        public TypeInjector<CodeContext> InjectType(string value)
        {
            return new TypeInjector<CodeContext>(this,SyntaxFactory.ParseTypeName(value));
        }
        public TypeInjector<CodeContext> InjectType(TypeSyntax value)
        {
            return new TypeInjector<CodeContext>(this,value);
        }
        public Injector<CodeContext> InjectId<T>(IdentifierNameSyntax value,out T id)
        {
            id = default;
            return new Injector<CodeContext>(this,value);
        }
        public Injector<CodeContext> InjectId<T>(string value,out T id)
        {
            id = default;
            return new Injector<CodeContext>(this, SyntaxFactory.IdentifierName(value));
        }
        
        public CodeContext StartOrContinueWith(string code)
        {
            var block = SyntaxFactory.ParseStatement($"{{{code}}}") as BlockSyntax;
            this.statements = this.statements.Concat(block.Statements);
            return this;
        }
        public CodeContext StartOrContinueWith(ParenthesizedLambdaExpressionSyntax code)
        {
            if (code.Block != null)
                statements = statements.Concat(code.Block.Statements);
            else
                statements = statements.Append(SyntaxFactory.ExpressionStatement(code.ExpressionBody));
            return this;
        }
        public CodeContext StartOrContinueWith(Action code) => throw new NonIntendedException();
        public CodeContext StartOrContinueWith<T1>(Action<T1> code) => throw new NonIntendedException();
        public CodeContext StartOrContinueWith<T1,T2>(Action<T1,T2> code) => throw new NonIntendedException();
        public CodeContext StartOrContinueWith<T1,T2,T3>(Action<T1,T2,T3> code) => throw new NonIntendedException();

    }

    public sealed class CodeContext<TResult> : BaseCodeContext
    {
        public CodeContext<TResult> StartOrContinueWith(string code)
        {
            var block = SyntaxFactory.ParseStatement($"{{{code}}}") as BlockSyntax;
            this.statements = this.statements.Concat(block.Statements);
            return this;
        }
        public CodeContext<TResult> StartOrContinueWith(ParenthesizedLambdaExpressionSyntax code)
        {
            if (code.Block != null)
                statements = statements.Concat(code.Block.Statements);
            else
                statements = statements.Append(SyntaxFactory.ExpressionStatement(code.ExpressionBody));
            return this;
        }
        public CodeContext<TResult> StartOrContinueWith(Action code) => throw new NonIntendedException();
        public CodeContext<TResult> StartOrContinueWith<T1>(Action<T1> code) => throw new NonIntendedException();
        public CodeContext<TResult> StartOrContinueWith<T1, T2>(Action<T1, T2> code) => throw new NonIntendedException();
        public CodeContext<TResult> StartOrContinueWith<T1,T2,T3>(Action<T1, T2, T3> code) => throw new NonIntendedException();
        public CodeContext<TResult> StartOrContinueWith(Func<TResult> code) => throw new NonIntendedException();
        public CodeContext<TResult> StartOrContinueWith<T1>(Func<T1,TResult> code) => throw new NonIntendedException();
        public CodeContext<TResult> StartOrContinueWith<T1, T2>(Func<T1,T2,TResult> code) => throw new NonIntendedException();
        public CodeContext<TResult> StartOrContinueWith<T1, T2, T3>(Func<T1,T2,T3,TResult> code) => throw new NonIntendedException();
    }

    public sealed class Injector<T>
        where T : BaseCodeContext
    {
        readonly T codeContext;
        readonly CSharpSyntaxNode node; 
        public Injector(T codeContext,CSharpSyntaxNode node)
        {
            this.codeContext = codeContext;
            this.node = node;
        }

        public T As(string identifier) 
        {
            codeContext.idSubstitutions.Add(identifier,node);
            return codeContext;
        }
    }

    public sealed class TypeInjector<T>
        where T : BaseCodeContext
    {
        readonly T codeContext;
        readonly CSharpSyntaxNode node;
        public TypeInjector(T codeContext, CSharpSyntaxNode node)
        {
            this.codeContext = codeContext;
            this.node = node;
        }

        public T As<Type>()
        {
            codeContext.idSubstitutions.Add(Extensions.GetCSharpName<Type>(), node);
            return codeContext;
        }
    }
}
