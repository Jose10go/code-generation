using CodeGeneration.CSharp;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using static CodeGen.CSharp.Context.CSharpContext;

namespace WithBodyTransformer
{
    public class WithBodyTransformer : CodeGenerationTransformer
    {
        public override void Transform()
        {
              Engine.Select<ParenthesizedLambdaExpressionSyntax,InvocationExpressionSyntax>()
                    .Where(x =>x.Parent.SemanticSymbol is IMethodSymbol)
                    .Where(x=>x.Parent.SemanticSymbol.Name is "WithBody" || 
                              x.Parent.SemanticSymbol.Name is "WithGet" || 
                              x.Parent.SemanticSymbol.Name is "WithSet")
                    .Where(x=>x.Parent.SemanticSymbol.ContainingType.Name is "IWithBody" ||
                              x.Parent.SemanticSymbol.ContainingType.Name is "IWithGetSet")
                    .Where(x=>x.Parent.Node.ArgumentList.Arguments.First().Expression==x.Node)
                        .Using(target => GetBody(target.Node), out var bodyKey)
                        .Execute((IReplaceExpression<ParenthesizedLambdaExpressionSyntax> cmd) => cmd.Get(bodyKey,out var stringBody)
                                                                                                     .With(stringBody));
        }

        private ExpressionSyntax GetBody(ParenthesizedLambdaExpressionSyntax node)
        {
            return SyntaxFactory.ParseExpression($"SyntaxFactory.Parse(\"{node}\") as ParenthesizedLambdaExpressionSyntax");
            
                //SyntaxFactory.BinaryExpression(SyntaxKind.AsExpression,
            //           SyntaxFactory.InvocationExpression(
            //           SyntaxFactory.MemberAccessExpression(SyntaxKind.SimpleMemberAccessExpression,
            //               SyntaxFactory.IdentifierName(nameof(SyntaxFactory)),
            //               SyntaxFactory.IdentifierName(nameof(SyntaxFactory.ParseExpression))),
            //           SyntaxFactory.ArgumentList().AddArguments(
            //               SyntaxFactory.Argument(
            //                   SyntaxFactory.LiteralExpression(SyntaxKind.StringLiteralExpression,
            //                       SyntaxFactory.Literal(node.ToString()))))),
            //           SyntaxFactory.IdentifierName(nameof(ParenthesizedLambdaExpressionSyntax)));
        }
    }
}
