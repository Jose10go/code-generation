using CodeGen.Context;
using CodeGen.Core.Attributes;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CodeGen.CSharp.Context
{
    public abstract partial class CSharpContext : CodeGenContext<Project, CSharpSyntaxNode, CompilationUnitSyntax, ISymbol>
    {
        public interface IReplaceExpression<Exp> : ICommand<Exp, ExpressionSyntax>,
                                                   IWith<IReplaceExpression<Exp>>,
                                                   IGet<IReplaceExpression<Exp>,Exp>
            where Exp:ExpressionSyntax
        {
        }

        [Command]
        public class ReplaceExpressionCommand<Exp> : IReplaceExpression<Exp>
            where Exp:ExpressionSyntax
        {
            public ISingleTarget<Exp> SingleTarget { get; set; }
            ExpressionSyntax IWith<IReplaceExpression<Exp>>.NewExpression { get; set; }
        }

    }
}
