using CodeGen.CSharp.Context.DocumentEdit;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CodeGeneration.CSharp
{
    public class WithBodyGenerator : CodeGenerationTask
    {
        public override void DoWork()
        {
            Engine.Select<InvocationExpressionSyntax>()
                     .Where((symbol, node) =>
                     {
                         var s = (IMethodSymbol)symbol;
                         if (s is null)
                             return false;
                         return s.Name == "f" && symbol.ContainingType.Name == "A";
                     })
                     .Execute<CSharpContextDocumentEditor.IReplaceInvocation>()
                     .WithNewArgument(0, (x) =>
                     {
                         var lambda = x.ArgumentList.Arguments[0].Expression as ParenthesizedLambdaExpressionSyntax;
                         var bodycode = SyntaxFactory.Literal(lambda.Body.ToString().Replace("@this", "this"));
                         return SyntaxFactory.Argument(SyntaxFactory.LiteralExpression(SyntaxKind.StringLiteralExpression, bodycode));
                     })
                     .Go();
        }
    }
}
