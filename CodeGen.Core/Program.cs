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
using CodeGen.Context.CSharp;

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

            var resolver = new ICSharpContext<DocumentEditor>.CSharpAutofacResolver();
            var engine = new ICSharpContext<DocumentEditor>.DocumentEditingCodeGenerationEngine(solution, resolver);
            var cmd =  engine.Select<MethodDeclarationSyntax>()
                      .Where(x => true)
                      .Execute<ICSharpContext<DocumentEditor>.CloneCommand<MethodDeclarationSyntax>, ICSharpContext<DocumentEditor>.MethodCloneCommandBuilder>()
                      .WithNewName(m => m.Identifier.Text + "_generated")
                      .Build();

            var result = cmd.Handler.ProcessDocument(editor);
            Console.WriteLine(result.ToString());

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