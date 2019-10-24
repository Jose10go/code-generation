using CodeGen.Context.CSharp.DocumentEdit;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Editing;
using Microsoft.CodeAnalysis.CSharp;
using System.Linq;
using Microsoft.Build.Locator;
using Xunit;
using System.IO;
using Microsoft.CodeAnalysis.MSBuild;

namespace Tests
{
    public class UnitTest
    {
        MSBuildWorkspace workspace;
        Solution solution;
        Project project;
        CSharpContextDocumentEditor.CSharpAutofacResolver resolver;
        CSharpContextDocumentEditor.DocumentEditingCodeGenerationEngine engine;
        public UnitTest()
        {
            var instance = MSBuildLocator.QueryVisualStudioInstances().First();
            MSBuildLocator.RegisterInstance(instance);

            workspace = MSBuildWorkspace.Create();
            workspace.WorkspaceFailed += (sender, args) =>
                                            workspace.Diagnostics.Add(args.Diagnostic);

            solution = workspace.CurrentSolution;
            project = workspace.OpenProjectAsync(@"..\..\..\Tester.csproj").Result;

            resolver = new CSharpContextDocumentEditor.CSharpAutofacResolver();
            engine = new CSharpContextDocumentEditor.DocumentEditingCodeGenerationEngine(solution);
        }

        private SyntaxTree ParseOut(string path)
        {
            using (FileStream f = new FileStream(Path.Combine("..", "..", "..", path, "out.cs"), FileMode.Open))
            using (StreamReader reader = new StreamReader(f))
            {
                var text = reader.ReadToEnd();
                return SyntaxFactory.ParseSyntaxTree(text);
            }
        }

        //[Fact]
        //public void CloneMethodUnDeclarativeWay()
        //{
        //    string path = Path.Combine("Examples", "CloneMethod");
        //    Document document_in = project.Documents.First(doc => doc.Folders.Aggregate((x, y) => Path.Combine(x, y)) == path && doc.Name == "in.cs");
        //    var editor = DocumentEditor.CreateAsync(document_in).Result;

        //    var target = new CSharpContext<DocumentEditor>.CSharpTarget<MethodDeclarationSyntax>((method) => true);
        //    var cloneCommand = new CSharpContext<DocumentEditor>.MethodCloneCommandBuilder()
        //        .WithNewName((method) => method.Identifier.Text + "_generated")
        //        .Build();
        //    var handler = new MethodCloneCommandHandler() { Target = target, Command = cloneCommand };

        //    DocumentEditor result = handler.ProcessDocument(editor);

        //    editor.GetChangedDocument().TryGetSyntaxTree(out var st);
        //    var st2 = ParseOut(path);
        //    Assert.True(st.IsEquivalentTo(st2));
        //}



        [Fact]
        public void CloneClass()
        {
            string path = Path.Combine("Examples", "CloneClass");
            Document document_in = project.Documents.First(doc => 
            doc.Folders.Aggregate((x, y) => Path.Combine(x, y)) == path && doc.Name == "in.cs");
            var editor = DocumentEditor.CreateAsync(document_in).Result;

            engine.Select<ClassDeclarationSyntax>()
                .Where(x => true)
                .Execute<CSharpContextDocumentEditor.ClassCloneCommand, CSharpContextDocumentEditor.IClassCloneCommandBuilder>()
                .WithNewName(m => m.Identifier.Text + "_generated") 
                .Go(editor);

            editor.GetChangedDocument().TryGetSyntaxTree(out var st);
            var st2 = ParseOut(path);
            Assert.True(st.IsEquivalentTo(st2));
        }

        [Fact]
        public void CloneMethod()
        {
            string path = Path.Combine("Examples", "CloneMethod");
            Document document_in = project.Documents.First(doc => doc.Folders.Aggregate((x, y) => Path.Combine(x, y)) == path && doc.Name == "in.cs");
            var editor = DocumentEditor.CreateAsync(document_in).Result;

            engine.Select<MethodDeclarationSyntax>()
                .Where(x => true)
                .Execute<CSharpContextDocumentEditor.MethodCloneCommand, CSharpContextDocumentEditor.IMethodCloneCommandBuilder>()
                .WithNewName(m => m.Identifier.Text + "_generated")
                .Go(editor);

            editor.GetChangedDocument().TryGetSyntaxTree(out var st);
            var st2 = ParseOut(path);
            Assert.True(st.IsEquivalentTo(st2));
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
