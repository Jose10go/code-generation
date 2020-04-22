using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.CSharp;
using Xunit;
using System.IO;
using static CodeGen.CSharp.Context.CSharpContext;
using Xunit.Abstractions;
using System.Linq;
using System;

namespace Tests
{
    public class CommandTests:IClassFixture<TestDocumentEditingCodeGenerationEngine>
    {
        readonly CSharpCodeGenerationEngine engine;
        readonly ITestOutputHelper output;
        string ProjectPath => engine.CurrentProject.FilePath;

        public CommandTests(ITestOutputHelper output,TestDocumentEditingCodeGenerationEngine engine)
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
            string inpath = Path.Combine(Path.GetDirectoryName(ProjectPath), "CloneClass", "in.cs");
            string outpath = Path.Combine(Path.GetDirectoryName(ProjectPath), "CloneClass", "out.cs");

            engine.Select<ClassDeclarationSyntax>()
                  .Where(x => x.DocumentPath == inpath)
                  .Using(x=>x.Node.Identifier.Text,out var keyName)
                  .Execute((ICloneClass cmd)=>cmd.Get(keyName,out var name)
                                                 .WithName(name + "_generated")
                                                 .MakePublic());

            Document document_in = engine.CurrentProject.Documents.First(x=>x.FilePath==inpath);
            engine.CurrentProject.GetDocument(document_in.Id).TryGetSyntaxTree(out var st1);
            var st2 = ParseFile(outpath);
            Assert.True(st1.IsEquivalentTo(st2));
        }

        [Fact]
        public void CloneMethod()
        {
            string inpath = Path.Combine(Path.GetDirectoryName(ProjectPath), "CloneMethod", "in.cs");
            string outpath = Path.Combine(Path.GetDirectoryName(ProjectPath), "CloneMethod", "out.cs");
            
            engine.Select<MethodDeclarationSyntax>()
                  .Where(x => x.DocumentPath == inpath)
                  .Using(x=>x.Node.Identifier.Text,out var keyName)
                  .Execute((ICloneMethod cmd)=>cmd.Get(keyName,out var name)
                                                  .WithName(name + "_generated")
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
        public void CreateMultipleMethods()
        {
            string inpath = Path.Combine(Path.GetDirectoryName(ProjectPath), "CreateMultipleMethods", "in.cs");
            string outpath = Path.Combine(Path.GetDirectoryName(ProjectPath), "CreateMultipleMethods", "out.cs");

            var _class = engine.SelectNew(inpath)
                               .Execute((ICreateNamespace cmd) => cmd.WithName("MultipleMethods"))
                               .Execute((ICreateClass cmd) => cmd.WithName("A")
                                                                 .MakePublic());
            for (int i = 0; i < 99; i++)
            {
                _class.Execute((ICreateMethod cmd) => cmd.WithName("returns" + i)
                                                         .WithAttributes("GeneratedCode")
                                                         .Returns<int>()
                                                         .MakeStatic()
                                                         .WithBody($"{{return {i};}}"));
            } 

            Document document_in = engine.CurrentProject.Documents.First(x => x.FilePath == inpath);
            engine.CurrentProject.GetDocument(document_in.Id).TryGetSyntaxTree(out var st1);
            var st2 = ParseFile(outpath);
            Assert.True(st1.IsEquivalentTo(st2));
        }

        [Fact]
        public void ReplaceInvocation()
        {
            string inpath = Path.Combine(Path.GetDirectoryName(ProjectPath), "ReplaceInvocation", "in.cs");
            string outpath = Path.Combine(Path.GetDirectoryName(ProjectPath), "ReplaceInvocation", "out.cs");

            engine.Select<ParenthesizedLambdaExpressionSyntax, ArgumentSyntax, InvocationExpressionSyntax>()
                  .Where(x => { 
                      var s = (IMethodSymbol)x.Grandparent.SemanticSymbol;
                      if (s is null)
                          return false;
                      return s.Name == "f" && s.ContainingType.Name == "A";})
                  .Where(x => x.DocumentPath == inpath)
                  .Using(target => target.Node.Body.ToString().Replace("@this", "this"), out var bodyKey)
                  .Execute((IReplaceExpression<ParenthesizedLambdaExpressionSyntax> cmd) => cmd.Get(bodyKey, out var stringBody)
                                                                                               .With(SyntaxFactory.ParseExpression($"\"{stringBody}\"")));
                  

            Document document_in = engine.CurrentProject.Documents.First(x => x.FilePath == inpath);
            engine.CurrentProject.GetDocument(document_in.Id).TryGetSyntaxTree(out var st1);
            var st2 = ParseFile(outpath);
            Assert.True(st1.IsEquivalentTo(st2));
        }

        [Fact] 
        public void HelloWorldTest() 
        {
            string outpath = Path.Combine(Path.GetDirectoryName(ProjectPath), "HelloWorld", "out.cs");
            string inpath = Path.Combine(Path.GetDirectoryName(ProjectPath), "HelloWorld", "in.cs");

            engine.SelectNew(inpath)
                  .Execute((ICreateNamespace cmd) => cmd.WithName("HelloWorld"))
                  .Execute((ICreateClass cmd) => cmd.WithName("Program")
                                                    .MakeStatic())
                  .Execute((ICreateMethod cmd) => cmd.WithName("Main")
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
