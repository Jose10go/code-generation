using CodeGen.CSharp.Context.DocumentEdit;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.CSharp;
using System.Linq;
using Xunit;
using System.IO;
using Microsoft.CodeAnalysis.MSBuild;
using static CodeGen.CSharp.Context.DocumentEdit.CSharpContextDocumentEditor;
using Microsoft.Build.Locator;

namespace Tests
{
    public class UnitTest
    {
        MSBuildWorkspace workspace;
        string slnPath;
        Solution solution;
        DocumentEditingCodeGenerationEngine engine;
        CSharpContextDocumentEditor.CSharpAutofacResolver resolver;
        
        public UnitTest()
        {
            var instance = MSBuildLocator.RegisterDefaults();

            workspace = MSBuildWorkspace.Create();
            workspace.WorkspaceFailed += (sender, args) =>
                                            workspace.Diagnostics.Add(args.Diagnostic);

            slnPath = Path.GetFullPath(Path.Combine("..", "..", "..", "Examples"));
            solution = workspace.OpenSolutionAsync(Path.Combine(slnPath, "Examples.sln")).Result;
            resolver = new CSharpContextDocumentEditor.CSharpAutofacResolver();
            engine = new DocumentEditingCodeGenerationEngine(solution.Projects.First());
        }

        private SyntaxTree ParseFile(string path)
        {
            using (FileStream f = new FileStream(path, FileMode.Open))
                using (StreamReader reader = new StreamReader(f))
                {
                    var text = reader.ReadToEnd();
                    return SyntaxFactory.ParseSyntaxTree(text);
                }
        }

        [Fact]
        public void CloneClass()
        {
            string inpath = Path.Combine(slnPath, "CloneClass", "in.cs");
            string outpath = Path.Combine(slnPath, "CloneClass", "out.cs");
            DocumentId document_id = solution.GetDocumentIdsWithFilePath(inpath).First();
            Document document_in = solution.GetDocument(document_id);
            
            engine.Select<ClassDeclarationSyntax>()
                    .Where(x => true)
                        .Execute<CSharpContextDocumentEditor.IClassClone>()
                            .WithNewName(m => m.Identifier.Text + "_generated")
                  .Go();

            engine.CurrentProject.GetDocument(document_in.Id).TryGetSyntaxTree(out var st1); 
            var st2 = ParseFile(outpath);
            Assert.True(st1.IsEquivalentTo(st2));
        }

        [Fact]
        public void CloneMethod()
        {
            string inpath = Path.Combine(slnPath,"CloneMethod","in.cs");
            string outpath = Path.Combine(slnPath,"CloneMethod","out.cs");
            DocumentId document_id = solution.GetDocumentIdsWithFilePath(inpath).First();
            Document document_in = solution.GetDocument(document_id);
            engine.Select<MethodDeclarationSyntax>()
                    .Where(x => true)
                        .Execute<CSharpContextDocumentEditor.IMethodClone>()
                            .WithNewName(m => m.Identifier.Text + "_generated")
                            .WithBody("{Console.WriteLine(\"hello my friend.\");}")
                            //.WithBody((dynamic @this)=>{ System.Console.WriteLine("hello my friend.");})//this is the best idea ever
                    .Go();

            engine.CurrentProject.GetDocument(document_in.Id).TryGetSyntaxTree(out var st1);
            var st2 = ParseFile(outpath);
            Assert.True(st1.IsEquivalentTo(st2));
        }

        [Fact]
        public void ReplaceInvocation()
        {
            string inpath = Path.Combine(slnPath, "ReplaceInvocation", "in.cs");
            string outpath = Path.Combine(slnPath, "ReplaceInvocation", "out.cs");
            DocumentId document_id = solution.GetDocumentIdsWithFilePath(inpath).First();
            Document document_in = solution.GetDocument(document_id);
            engine.Select<InvocationExpressionSyntax>()
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

            engine.CurrentProject.GetDocument(document_in.Id).TryGetSyntaxTree(out var st1);
            var st2 = ParseFile(outpath);
            Assert.True(st1.IsEquivalentTo(st2));
        }

        //[Fact]
        //public void CreateClass()
        //{
        //    string path = Path.Combine("Examples", "CreateClass");
        //    Document document_in = project.Documents.First(doc => doc.Folders.Aggregate((x, y) => Path.Combine(x, y)) == path && doc.Name == "in.cs");
        //    var editor = DocumentEditor.CreateAsync(document_in).Result;

        //    engine.Select<NamespaceDeclarationSyntax>()
        //       .Execute<CSharpContextDocumentEditor.CreateClassComand, CSharpContextDocumentEditor.ClassCreateCommandBuilder>()
        //       .WithAccesModifier("public")
        //       .MakePartial()
        //       .MakeStatic()
        //       .WithName("B")
        //       .Go(editor);

        //    editor.GetChangedDocument().TryGetSyntaxTree(out var st);
        //    var st2 = ParseOut(path);
        //    Assert.True(st.IsEquivalentTo(st2));
        //}

    }
}
