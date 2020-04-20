using CodeGeneration.CSharp;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System;
using System.Linq;
using static CodeGen.CSharp.Context.CSharpContext;

namespace WithBodyTransformer
{
    public class WithBodyTransformer : CodeGenerationTransformer
    {
        public override void Transform()
        {
            Engine.Select<ParenthesizedLambdaExpressionSyntax, ArgumentSyntax, InvocationExpressionSyntax>()
                  .Where(single =>
                  {
                      var s = (IMethodSymbol)single.SemanticSymbol;
                      if (s is null)
                          return false;
                      return s.Name == "WithBody" && s.ContainingType.Name == "IWithBody" && !s.Parameters.Any(x => x.Type.Name == "string");
                  })
                  .Using(target => target.Node.Body.ToString().Replace("@this", "this"), out var bodyKey)
                  .Execute((IReplaceExpression<ParenthesizedLambdaExpressionSyntax> cmd) => cmd.Get(bodyKey, out var stringBody)
                                                                                               .With(SyntaxFactory.ParseExpression($"\"{stringBody}\"")));
        }
    }
}
