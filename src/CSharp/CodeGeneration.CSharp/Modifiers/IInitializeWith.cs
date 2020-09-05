using CodeGen.Attributes;
using CodeGen.Context;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CodeGen.CSharp.Context
{
    public abstract partial class CSharpContext : CodeGenContext<Project, CSharpSyntaxNode, CompilationUnitSyntax, ISymbol>
    {
        [CommandModifier]
        public interface IInitializeWith<TCommand>
            where TCommand:Core.ICommand
        {
            ExpressionSyntax InitializerExpression { get; set; }

            TCommand InitializeWith(string exp)
            {
                this.InitializerExpression = SyntaxFactory.ParseExpression(exp);
                return (TCommand)this;
            }

            TCommand InitializeWith(CodeContext exp)
            {
                this.InitializerExpression = exp.GetCode() as ExpressionSyntax;
                return (TCommand)this;
            }

        }
    }
}
