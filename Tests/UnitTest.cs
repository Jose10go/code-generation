using CodeGen.Context.CSharp;
using CodeGen.Context.CSharp.DocumentEdit;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Editing;
using Microsoft.CodeAnalysis.MSBuild;
using System;
using System.Linq;
using static CodeGen.Context.CSharp.DocumentEdit.CSharpContextDocumentEditor;
using Microsoft.Build.Locator;
using Xunit;

namespace Tester
{
    public class UnitTest
    {
        MSBuildWorkspace workspace;
        Solution solution;
        CSharpContextDocumentEditor.CSharpAutofacResolver resolver;
        CSharpContextDocumentEditor.DocumentEditingCodeGenerationEngine engine;
        public UnitTest()
        {
            var instance = MSBuildLocator.QueryVisualStudioInstances().First();
            MSBuildLocator.RegisterInstance(instance);
            //Environment.SetEnvironmentVariable("VSINSTALLDIR", @"C:\Program Files (x86)\Microsoft Visual Studio\2017\Enterprise");
            //Environment.SetEnvironmentVariable("VisualStudioVersion", @"15.0");
            workspace = MSBuildWorkspace.Create();
            workspace.WorkspaceFailed += (sender, args) => 
                                                    Console.WriteLine(args.Diagnostic.Message);
            solution = workspace.CurrentSolution;
            resolver = new CSharpContextDocumentEditor.CSharpAutofacResolver();
            engine = new CSharpContextDocumentEditor.DocumentEditingCodeGenerationEngine(solution);
        }

        [Fact]
        public void CloneMethodUnDeclarativeWay()
        {
            var project = workspace.OpenProjectAsync(@"C:\Users\Jose10go\Desktop\Tesis\CodeGeneration\csharp_code_generation-master\Tests\Tests.csproj").Result;
            Document document = project.Documents.First(doc => doc.Name == "in.cs");
            var editor = DocumentEditor.CreateAsync(document).Result;

            var target = new CSharpContext<DocumentEditor>.CSharpTarget<MethodDeclarationSyntax>((method) => true);
            var cloneCommand = new CSharpContext<DocumentEditor>.MethodCloneCommandBuilder()
                .WithNewName((method) => method.Identifier.Text + "_generated")
                .Build();
            var handler = new MethodCloneCommandHandler() { Target = target, Command = cloneCommand };

            DocumentEditor result = handler.ProcessDocument(editor);
            Console.WriteLine(editor.GetChangedDocument().GetTextAsync().Result);

        }

        //[TestMethod]
        //public async void CloneMethod()
        //{
        //    var project = await workspace.OpenProjectAsync(@"Tester.csproj");
        //    Document document = project.Documents.First(doc => doc.Name == "in.cs");
        //    var editor = DocumentEditor.CreateAsync(document).Result;

        //    engine.Select<MethodDeclarationSyntax>()
        //        .Where(x => true)
        //        .Execute<CSharpContextDocumentEditor.CloneCommand<MethodDeclarationSyntax>, CSharpContextDocumentEditor.MethodCloneCommandBuilder>()
        //        .WithNewName(m => m.Identifier.Text + "_generated")
        //        .Go(editor);
        //}

        //[TestMethod]
        //public async void CloneClass()
        //{
        //    var project = await workspace.OpenProjectAsync(@"Tester.csproj");
        //    Document document = project.Documents.First(doc => doc.Name == "in.cs");
        //    var editor = DocumentEditor.CreateAsync(document).Result;

        //    engine.Select<ClassDeclarationSyntax>()
        //        .Where(x => true)
        //        .Execute<CSharpContextDocumentEditor.CloneCommand<ClassDeclarationSyntax>, CSharpContextDocumentEditor.ClassCloneCommandBuilder>()
        //        .WithNewName(m => m.Identifier.Text + "_generated")
        //        .Go(editor);
        //}

        //[TestMethod]
        //public async void CreateClass()
        //{
        //    var project = await workspace.OpenProjectAsync(@"Tester.csproj");
        //    Document document = project.Documents.First(doc => doc.Name == "in.cs");
        //    var editor = DocumentEditor.CreateAsync(document).Result;

        //    engine.Select<NamespaceDeclarationSyntax>()
        //       .Execute<CSharpContextDocumentEditor.CreateClassComand, CSharpContextDocumentEditor.ClassCreateCommandBuilder>()
        //       .WithAccesModifier("private")
        //       .MakePartial()
        //       .MakeStatic()
        //       .WithName("B")
        //       .Go(editor);
        //}

    }
}
