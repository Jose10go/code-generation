using CodeGen.CSharp.Context.DocumentEdit;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.CSharp;
using System.Linq;
using Xunit;
using System.IO;
using static CodeGen.CSharp.Context.DocumentEdit.CSharpContextDocumentEditor;
using Xunit.Abstractions;

namespace Tests
{
    public class UnitTest:IClassFixture<TestDocumentEditingCodeGenerationEngine>
    {
        readonly DocumentEditingCodeGenerationEngine engine;
        readonly ITestOutputHelper output;
        string projectPath => engine.CurrentProject.FilePath;

        public UnitTest(ITestOutputHelper output,TestDocumentEditingCodeGenerationEngine engine)
        {
            this.output = output;
            this.engine = engine;
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
            string inpath = Path.Combine(Path.GetDirectoryName(projectPath), "CloneClass", "in.cs");
            string outpath = Path.Combine(Path.GetDirectoryName(projectPath), "CloneClass", "out.cs");

            engine.Select<ClassDeclarationSyntax>()
                  .Where(x => x.DocumentPath == inpath)
                  .Execute((CSharpContextDocumentEditor.IClassClone cmd)=>cmd.WithNewName(m => m.Identifier.Text + "_generated")
                                                                             .MakePublic());

            Document document_in = engine.CurrentProject.Documents.First(x=>x.FilePath==inpath);
            engine.CurrentProject.GetDocument(document_in.Id).TryGetSyntaxTree(out var st1);
            var st2 = ParseFile(outpath);
            Assert.True(st1.IsEquivalentTo(st2));
        }

        [Fact]
        public void CloneMethod()
        {
            string inpath = Path.Combine(Path.GetDirectoryName(projectPath), "CloneMethod", "in.cs");
            string outpath = Path.Combine(Path.GetDirectoryName(projectPath), "CloneMethod", "out.cs");
            
            engine.Select<MethodDeclarationSyntax>()
                  .Where(x => x.DocumentPath == inpath)
                  .Execute((CSharpContextDocumentEditor.IMethodClone cmd)=>cmd.WithNewName(m => m.Identifier.Text + "_generated")
                                                                              .MakePublic()
                                                                              .WithBody("{Console.WriteLine(\"hello my friend.\");}"));

            //This Comment is preserved to mark the moment :)
            //.WithBody((dynamic @this)=>{ System.Console.WriteLine("hello my friend.");})//this is the best idea ever...

            Document document_in = engine.CurrentProject.Documents.First(x => x.FilePath == inpath); 
            engine.CurrentProject.GetDocument(document_in.Id).TryGetSyntaxTree(out var st1);
            var st2 = ParseFile(outpath);
            Assert.True(st1.IsEquivalentTo(st2));
        }

        [Fact]
        public void ReplaceInvocation()
        {
            string inpath = Path.Combine(Path.GetDirectoryName(projectPath), "ReplaceInvocation", "in.cs");
            string outpath = Path.Combine(Path.GetDirectoryName(projectPath), "ReplaceInvocation", "out.cs");

            engine.Select<ParenthesizedLambdaExpressionSyntax, ArgumentSyntax, InvocationExpressionSyntax>()
                  .Where(x => { 
                      var s = (IMethodSymbol)x.Grandparent.SemanticSymbol;
                      if (s is null)
                          return false;
                      return s.Name == "f" && s.ContainingType.Name == "A";})
                  .Where(x => x.DocumentPath == inpath)
                  .Using(target => target.Node.Body.ToString().Replace("@this", "this"), out var bodyKey)
                  .Execute((CSharpContextDocumentEditor.IReplaceExpression<ParenthesizedLambdaExpressionSyntax> cmd) => cmd.Get(bodyKey, out var stringBody)
                                                                                                                           .With(SyntaxFactory.ParseExpression($"\"{stringBody}\"")));
                  

            Document document_in = engine.CurrentProject.Documents.First(x => x.FilePath == inpath);
            engine.CurrentProject.GetDocument(document_in.Id).TryGetSyntaxTree(out var st1);
            var st2 = ParseFile(outpath);
            Assert.True(st1.IsEquivalentTo(st2));
        }

        [Fact] 
        public void HelloWorldTest() 
        {
            string outpath = Path.Combine(Path.GetDirectoryName(projectPath), "HelloWorld", "out.cs");
            string inpath = Path.Combine(Path.GetDirectoryName(projectPath), "HelloWorld", "in.cs");

            engine.SelectNew(inpath)
                  .Execute((CSharpContextDocumentEditor.ICreateNamespace cmd) => cmd.WithName("HelloWorld"))
                  .Execute((CSharpContextDocumentEditor.ICreateClass cmd) => cmd.WithName("Program")
                                                                                .MakeStatic())
                  .Execute((CSharpContextDocumentEditor.ICreateMethod cmd) => cmd.WithName("Main")
                                                                                 .MakeStatic()
                                                                                 .WithBody("{System.Console.WriteLine(\"Hello World!!!\"); }"));

            Document document_in = engine.CurrentProject.Documents.First(x => x.FilePath == inpath);
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
