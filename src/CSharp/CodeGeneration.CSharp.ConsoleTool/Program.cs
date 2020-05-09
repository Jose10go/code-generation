using Buildalyzer;
using Buildalyzer.Workspaces;
using Microsoft.CodeAnalysis;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using System.Reflection;
using static CodeGen.CSharp.Context.CSharpContext;

namespace CodeGeneration.CSharp.Precompilation
{
    static class Program
    {
        [SuppressMessage("Major Code Smell", "S1144:Unused private types or members should be removed", Justification = "System.CommandLine.DragonFruits")]
        static void Main(string project,string[] transformers)
        {
            AnalyzerManager manager = new AnalyzerManager();
            ProjectAnalyzer analyzer = manager.GetProject(project);
            AdhocWorkspace workspace = new AdhocWorkspace();

            //workspace.WorkspaceFailed += (sender, args) => throw new Exception("Aleluya");
            //TODO:should be loading project references???

            var Project = analyzer.AddToWorkspace(workspace);
            if (Project is null)
                throw new ArgumentException($"Project is null. ({project})");

            var resolver = new CSharpAutofacResolver();
            var Engine = new CSharpCodeGenerationEngine(Project,resolver);
            foreach (var item in transformers)
            {
                var assembly=LoadAssembly(item);
                var transformer = LoadTransformer(assembly,Engine);
                transformer.Transform();
            }
            var changes = Engine.CurrentProject.GetChanges(Project);
            Procces(Engine,project,changes.GetAddedDocuments(), "Added");
            Procces(Engine,project,changes.GetChangedDocuments(), "Updated");
        }

        private static void Procces(CSharpCodeGenerationEngine engine, string project, IEnumerable<DocumentId> docs, string status)
        {
            foreach (var docId in docs)
            {
                var doc = engine.CurrentProject.GetDocument(docId);
                var text = doc.GetSyntaxRootAsync().Result.NormalizeWhitespace().ToFullString();//TODO: make async
                var relativePath = Path.GetRelativePath(Path.GetDirectoryName(project), doc.FilePath);
                var newPath = Path.Combine(Path.GetDirectoryName(project),"obj","Transformers","CSharp", relativePath);
                Directory.CreateDirectory(Path.GetDirectoryName(newPath));
                File.WriteAllText(newPath, text);
                Console.WriteLine(new TaskData() { Kind = "Compile", FilePath = relativePath, Status = status }+";");
            }
        }

        private static CodeGenerationTransformer LoadTransformer(Assembly assembly,CSharpCodeGenerationEngine Engine)
        {
            var type=assembly.GetTypes().First(x => x.IsSubclassOf(typeof(CodeGenerationTransformer)));//TODO: Why only one transformer by assembly
            if (type is null) 
                throw new ArgumentException($"There is no transformer on assembly {assembly.FullName}.");
            var transformer = Activator.CreateInstance(type);
            if (transformer is null)
                throw new ArgumentException($"Cant get instance of {type.Name}.");

            CodeGenerationTransformer trans = transformer as CodeGenerationTransformer;
            trans.Engine = Engine;
            return transformer as CodeGenerationTransformer;
        }

        private static Assembly LoadAssembly(string path)
        {
            var asembly = Assembly.LoadFrom(path);
            if (asembly is null)
                throw new ArgumentException($"Cant load assembly from {path}.");
            return asembly;
        }

    }
}
