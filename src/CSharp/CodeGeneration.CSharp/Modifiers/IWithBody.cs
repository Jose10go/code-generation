using CodeGen.Attributes;
using CodeGen.Context;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CodeGen.CSharp.Context
{
    public abstract partial class CSharpContext : CodeGenContext<Project, CSharpSyntaxNode,CompilationUnitSyntax, ISymbol>
    {
        [CommandModifier]
        public interface IWithBody<TCommand>
            where TCommand:Core.ICommand
        {
            BlockSyntax BlockBody { get; set; }
            ArrowExpressionClauseSyntax ExpressionBody { get; set; }

            TCommand WithBody(CodeContext codeContext)
            {
                var body = codeContext.GetCode();
                BlockBody = body as BlockSyntax;
                ExpressionBody = SyntaxFactory.ArrowExpressionClause(body as ExpressionSyntax);
                return (TCommand)this;
            }
        }
    }
}
