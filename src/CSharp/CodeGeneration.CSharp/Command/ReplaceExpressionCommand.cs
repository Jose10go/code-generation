using CodeGen.Context;
using CodeGen.Core.Attributes;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System;

namespace CodeGen.CSharp.Context
{
    public abstract partial class CSharpContext<TProcessEntity> : CodeGenContext<Project, CSharpSyntaxNode, CompilationUnitSyntax,ISymbol, TProcessEntity>
    {
        public interface IReplaceExpression : ICommand<ExpressionSyntax, ExpressionSyntax>,
                                              IWith<IReplaceExpression, ExpressionSyntax>
        {
        }

        [Command]
        public class ReplaceExpressionCommand : CSharpMultipleTarget<ExpressionSyntax>, IReplaceExpression
        {
            public ITarget<ExpressionSyntax> Target { get ; set ; }
            Func<ExpressionSyntax, ExpressionSyntax> IWith<IReplaceExpression, ExpressionSyntax>.NewExpression { get; set; }

            public ReplaceExpressionCommand(ICodeGenerationEngine engine):base(engine)
            {
            }
        }

    }
}
