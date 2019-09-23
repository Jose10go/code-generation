using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.MSBuild;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis.Editing;
using Autofac;

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
                var project = await workspace.OpenProjectAsync(@"..\..\CodeGen.Core.csproj");

                var compilation = await project.GetCompilationAsync();

                Console.WriteLine($"Diagnostics: {compilation.GetDiagnostics().Count()} \n" +
                    $"{compilation.GetDiagnostics().Select(diag => diag.GetMessage()).Aggregate((a, b) => $"{a}\n{b}")}");

                Console.WriteLine($"Syntax trees: {compilation.SyntaxTrees.Count()}");

                var ctx = new MyContext(project);
                //var compilation = await project.GetCompilationAsync();

                //var workspace = MSBuildWorkspace.Create();
                //var solution = await workspace.OpenSolutionAsync(@"E:\Tony\UH\Maestria\Tesis\project\CSharpCodeGeneration\CSharpCodeGeneration.sln");

                //Project project = solution.Projects.First(proj => proj.Name == "Tests");

                //var codegen = new CodeGenerator(new SolutionEditor(solution));


                //var handler = codegen.DIContainer.Resolve<ICommandHandler<CloneCommand<MethodDeclarationSyntax>, CSharpTarget<MethodDeclarationSyntax>,
                //    MethodDeclarationSyntax, DocumentEditor>>();

                var modifiedDocument = ctx.result.GetChangedDocument();

                var text = await modifiedDocument.GetTextAsync();

                Console.WriteLine(text);

            }
        public class MyContext:CodeGen.Context.CSharp.ICSharpContext<DocumentEditor>
        {
            public DocumentEditor result;
            public MyContext(Project project)
            {
                Document document = project.Documents.First(doc => doc.Name == "in.cs");

                var editor = DocumentEditor.CreateAsync(document).Result;
                var target = new CSharpTarget<MethodDeclarationSyntax>((method)=>true);
                var cloneCommand = new MethodCloneCommandBuilder()
                    .WithNewName((method) => method.Identifier.Text + "_generated")
                    .Build(); 
                var handler=new MethodCloneCommandHandler() { Target = target, Command = cloneCommand };

                result= handler.ProcessDocument(editor);
            }
        }
    }
}