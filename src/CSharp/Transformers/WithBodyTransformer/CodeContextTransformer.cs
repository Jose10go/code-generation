using CodeGeneration.CSharp;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using static CodeGen.CSharp.Context.CSharpContext;

namespace CodeContextTransformer
{
    public class CodeContextTransformer : CodeGenerationTransformer
    {
        public override void Transform()
        {
              Engine.Select<ParenthesizedLambdaExpressionSyntax,InvocationExpressionSyntax>()
                    .Where(x =>x.Parent.SemanticSymbol is IMethodSymbol)
                    .Where(x=>x.Parent.SemanticSymbol.Name is "StartOrContinueWith")
                    .Where(x=>x.Parent.SemanticSymbol.ContainingType.Name is "CodeContext") 
                    .Where(x=>x.Parent.Node.ArgumentList.Arguments.First().Expression==x.Node)
                    .Using(target => GetBody(target.Node), out var bodyKey)
                    .Execute((IReplaceExpression<ParenthesizedLambdaExpressionSyntax> cmd) => cmd.Get(bodyKey,out var stringBody)
                                                                                                 .With(stringBody));
        }

        private ExpressionSyntax GetBody(ParenthesizedLambdaExpressionSyntax node)
        {
            var code = node.ToString()
                .Replace("{", "{{")
                .Replace("}", "}}")
                .Replace("\"", "\"\"");
            return SyntaxFactory.ParseExpression(@$"@$""{code}""");
        }
    }
}
