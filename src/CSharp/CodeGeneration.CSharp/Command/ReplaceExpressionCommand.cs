//using CodeGen.Context;
//using CodeGen.Core.Attributes;
//using Microsoft.CodeAnalysis;
//using Microsoft.CodeAnalysis.CSharp;
//using Microsoft.CodeAnalysis.CSharp.Syntax;
//using System;

//namespace CodeGen.CSharp.Context
//{
//    public abstract partial class CSharpContext<TProcessEntity> : CodeGenContext<Project, CSharpSyntaxNode, CompilationUnitSyntax,ISymbol, TProcessEntity>
//    {
//        public interface IReplaceExpression : ICommand<ExpressionType>,
//                                              IWith<IReplaceExpression<TExpressionType>,TExpressionType>
//            where TExpressionType:ExpressionSyntax
//        {
//        }

//        [Command]
//        public class ReplaceExpressionCommand<TExpressionType> : IReplaceExpression<TExpressionType>
//            where TExpressionType:ExpressionSyntax
//        {
//            public ITarget<ExpressionSyntax> Target { get ; set ; }
//            Func<TExpressionType,TExpressionType> IWith<IReplaceExpression, ExpressionSyntax>.NewExpression { get; set; }
//        }

//    }
//}
