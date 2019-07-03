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
            var project = await workspace.OpenProjectAsync(@"E:\Tony\UH\Maestria\Tesis\project\CSharpCodeGeneration\CodeGen.Core\CodeGen.Core.csproj");

            var compilation = await project.GetCompilationAsync();

            Console.WriteLine($"Diagnostics: {compilation.GetDiagnostics().Count()} \n" +
                $"{compilation.GetDiagnostics().Select(diag => diag.GetMessage()).Aggregate((a, b) => $"{a}\n{b}")}");

            Console.WriteLine($"Syntax trees: {compilation.SyntaxTrees.Count()}");

            //var compilation = await project.GetCompilationAsync();

            //var workspace = MSBuildWorkspace.Create();
            //var solution = await workspace.OpenSolutionAsync(@"E:\Tony\UH\Maestria\Tesis\project\CSharpCodeGeneration\CSharpCodeGeneration.sln");

            //Project project = solution.Projects.First(proj => proj.Name == "Tests");

            //var codegen = new CodeGenerator(new SolutionEditor(solution));



            //var handler = codegen.DIContainer.Resolve<ICommandHandler<CloneCommand<MethodDeclarationSyntax>, CSharpTarget<MethodDeclarationSyntax>, 
            //    MethodDeclarationSyntax, DocumentEditor>>();

            //Document document = project.Documents.First(doc => doc.Name == "in.cs");

            //var editor = await DocumentEditor.CreateAsync(document);

            //var target = new CSharpTarget<MethodDeclarationSyntax>();
            //var cloneCommand = new MethodCloneCommandHandler() { Target = target, NamingTransformFunction = (method) => method.Identifier.Text + "_generated" };

            //editor = cloneCommand.ProcessDocument(editor);

            //var modifiedDocument = editor.GetChangedDocument();

            //var text = await modifiedDocument.GetTextAsync();

            //Console.WriteLine(text);

            //var compilation = await project.GetCompilationAsync();

            //foreach (var @class in compilation.GlobalNamespace.GetNamespaceMembers().SelectMany(x => x.GetMembers()))
            //{
            //    Console.WriteLine(@class.Name);
            //    Console.WriteLine(@class.ContainingNamespace.Name);
            //}

            //var classVisitor = new ClassVirtualizationVisitor();

            //foreach (var syntaxTree in compilation.SyntaxTrees)
            //{
            //    classVisitor.Visit(syntaxTree.GetRoot());
            //}

            //var classes = classVisitor.Classes;
        }

        //class ClassVirtualizationVisitor : CSharpSyntaxRewriter
        //{
        //    public ClassVirtualizationVisitor()
        //    {
        //        Classes = new List<ClassDeclarationSyntax>();
        //    }

        //    public List<ClassDeclarationSyntax> Classes { get; set; }

        //    public override SyntaxNode VisitClassDeclaration(ClassDeclarationSyntax node)
        //    {
        //        node = (ClassDeclarationSyntax)base.VisitClassDeclaration(node);
        //        Classes.Add(node); // save your visited classes
        //        return node;
        //    }
        //}
    }

    //interface ICommand
    //{
    //    IEnumerable<Func<object, bool>> filters { get; }
    //    void AddFilter(Func<object, bool> filter);
    //}

    //interface ICommand<in T> : ICommand
    //{
    //    new IEnumerable<Func<T, bool>> filters { get; }

    //}

    //class Command : ICommand<string>
    //{
    //    internal List<Func<object, bool>> filtersContainer;
    //    public IEnumerable<Func<string, bool>> filters => ((ICommand)this).filters;

    //    IEnumerable<Func<object, bool>> ICommand.filters => filtersContainer;

    //    public void AddFilter(Func<object, bool> filter)
    //    {
    //        filtersContainer.Add(filter);
    //    }
    //}

    //static class ICommandExtensors
    //{
    //    public static void AddFilter<T>(this ICommand<T> command, Func<T, bool> filter)
    //    {
    //        command.AddFilter((object o) => filter((T)o));
    //    }
    //}
}
