using System;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.MSBuild;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis.Editing;
using CodeGen.Context.CSharp.DocumentEdit;

namespace Tests
{
    class Program
    {
        private static void Main(string[] args)
        {
            Run(args).Wait();

            Console.ReadLine();

        }
        private async static Task Run(string[] args)
        {
            // In order to MSBuildWorkspace find MSBuild Tools
            //Environment.SetEnvironmentVariable("VSINSTALLDIR", @"C:\Program Files (x86)\Microsoft Visual Studio\2017\Enterprise");
            //Environment.SetEnvironmentVariable("VisualStudioVersion", @"15.0");

            var workspace = MSBuildWorkspace.Create();
            var solution = workspace.CurrentSolution;
            var project = await workspace.OpenProjectAsync(@"..\..\CodeGen.Core.csproj");

            Document document = project.Documents.First(doc => doc.Name == "in.cs");

            var editor = DocumentEditor.CreateAsync(document).Result;

            var resolver = new CSharpContextDocumentEditor.CSharpAutofacResolver();
            var engine = new CSharpContextDocumentEditor.DocumentEditingCodeGenerationEngine(solution);

            engine.Select<MethodDeclarationSyntax>()
                .Where(x => true)
                .Execute<CSharpContextDocumentEditor.CloneCommand<MethodDeclarationSyntax>, CSharpContextDocumentEditor.MethodCloneCommandBuilder>()
                .WithNewName(m => m.Identifier.Text + "_generated")
                .Go(editor);

            Console.WriteLine(editor.GetChangedDocument().GetTextAsync().Result);
        }

        //public class MyContext: CSharpContext<DocumentEditor>
        //{
        //    public DocumentEditor result;
        //    public MyContext(Project project)
        //    {
        //        Document document = project.Documents.First(doc => doc.Name == "in.cs");

        //        var editor = DocumentEditor.CreateAsync(document).Result;
        //        var target = new CSharpTarget<MethodDeclarationSyntax>((method)=>true);
        //        var cloneCommand = new MethodCloneCommandBuilder()
        //            .WithNewName((method) => method.Identifier.Text + "_generated")
        //            .Build(); 
        //        var handler=new MethodCloneCommandHandler() { Target = target, Command = cloneCommand };

        //        result= handler.ProcessDocument(editor);
        //    }
        //}
    }
}