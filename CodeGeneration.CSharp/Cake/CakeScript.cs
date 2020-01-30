using Cake.Core;
using Cake.Core.Annotations;
using CodeGen.CSharp.Context.DocumentEdit;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.MSBuild;
using static CodeGen.CSharp.Context.DocumentEdit.CSharpContextDocumentEditor;

namespace CodeGeneration.CSharp.Cake
{
    public static class CakeScript
    {
        [CakeMethodAlias]
        public static void Precompile(this ICakeContext context)
        {
            var workspace = MSBuildWorkspace.Create();
            workspace.WorkspaceFailed += (sender, args) =>
                                            workspace.Diagnostics.Add(args.Diagnostic);

            var solution = workspace.CurrentSolution;
            var resolver = new CSharpContextDocumentEditor.CSharpAutofacResolver();
            var engine = new DocumentEditingCodeGenerationEngine(solution);
            engine.Select<InvocationExpressionSyntax>()
                  .Where((symbol, node) =>
                  {
                      var s = (IMethodSymbol)symbol;
                      if (s is null)
                          return false;
                      return s.Name == "WithBody" && s.TypeParameters.Length > 0 && symbol.ContainingType.Name == "IWithBody";
                  })
                  .Execute<CSharpContextDocumentEditor.IReplaceInvocation>()
                  .WithNewArgument(0,(x) =>
                  {
                        var lambda = x.ArgumentList.Arguments[0].Expression as ParenthesizedLambdaExpressionSyntax;
                        var bodycode = SyntaxFactory.ParseToken(lambda.Body.ToString().Replace("@this", "this"));
                        return SyntaxFactory.Argument(SyntaxFactory.LiteralExpression(SyntaxKind.StringLiteralToken, bodycode));
                  })
                  .Go();

            //[CakePropertyAlias]
            //public static int TheAnswerToLife(this ICakeContext context)
            //{
            //    return 42;
            //}
        }
    }
}
