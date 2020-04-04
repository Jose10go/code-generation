using CodeGen.CSharp.Context.DocumentEdit;
using CodeGeneration.CSharp;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System;
using System.Linq;

namespace WithBodyTransformer
{
    public class WithBodyTransformer : CodeGenerationTransformer
    {
        public override void Transform()
        {
            Engine.Select<InvocationExpressionSyntax>()
                  .Where(single =>
                  {
                      var s = (IMethodSymbol)single.SemanticSymbol;
                      if (s is null)
                          return false;
                      return s.Name == "WithBody" && s.ContainingType.Name == "IWithBody" && !s.Parameters.Any(x=>x.Type.Name=="string");
                  })
                  .Execute<CSharpContextDocumentEditor.IReplaceInvocation>(
                    cmd=>cmd.WithNewArgument(0, (x) =>
                      {
                          var lambda = x.ArgumentList.Arguments[0].Expression as ParenthesizedLambdaExpressionSyntax;
                          var bodycode = SyntaxFactory.Literal(lambda.Body.ToString().Replace("@this", "this"));
                          return SyntaxFactory.Argument(SyntaxFactory.LiteralExpression(SyntaxKind.StringLiteralExpression, bodycode));
                      }));
        }
    }
}
