using CodeGen.CSharp.Context.DocumentEdit;
using Microsoft.Build.Locator;
using Microsoft.CodeAnalysis.MSBuild;
using System;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using System.Reflection;
using static CodeGen.CSharp.Context.DocumentEdit.CSharpContextDocumentEditor;

namespace CodeGeneration.CSharp.Precompilation
{
    static class Program
    {
        [SuppressMessage("Major Code Smell", "S1144:Unused private types or members should be removed", Justification = "System.CommandLine.DragonFruits")]
        static void Main(string project,string[] compiles,string[] transformers)
        {
            MSBuildLocator.RegisterDefaults();
            var workspace = MSBuildWorkspace.Create();
            workspace.WorkspaceFailed += (sender, args) =>
                                            workspace.Diagnostics.Add(args.Diagnostic);

            var Project = workspace.OpenProjectAsync(project).Result;
            if (Project == null)
                throw new Exception($"Project is null. ({project})");

            var resolver = new CSharpContextDocumentEditor.CSharpAutofacResolver();
            var Engine = new DocumentEditingCodeGenerationEngine(Project,resolver);

            foreach (var item in transformers)
            {
                var assembly=LoadAssembly(item);
                var transformer = LoadTransformer(assembly,Engine);
                transformer.Transform();
            }

            var changes = Engine.CurrentProject.GetChanges(Project);
            foreach (var docId in changes.GetAddedDocuments().Union(changes.GetChangedDocuments(true)))
            {
                var doc = Engine.CurrentProject.GetDocument(docId);
                var text = doc.GetTextAsync().Result;//TODO: make async
                Directory.CreateDirectory(Path.GetDirectoryName(doc.FilePath));
                File.WriteAllText(doc.FilePath, text.ToString());
                Console.WriteLine(Path.GetRelativePath(Path.GetDirectoryName(Engine.CurrentProject.FilePath), doc.FilePath));
            }

        }

        private static CodeGenerationTransformer LoadTransformer(Assembly assembly,DocumentEditingCodeGenerationEngine Engine)
        {
            var type=assembly.GetTypes().First(x => x.IsSubclassOf(typeof(CodeGenerationTransformer)));//TODO: Why only one transformer by assembly
            if (type is null) 
                throw new Exception($"There is no transformer on assembly {assembly.FullName}.");
            var transformer = Activator.CreateInstance(type);
            if (transformer is null)
                throw new Exception($"Cant get instance of {type.Name}.");

            CodeGenerationTransformer trans = transformer as CodeGenerationTransformer;
            trans.Engine = Engine;
            return transformer as CodeGenerationTransformer;
        }

        private static Assembly LoadAssembly(string path)
        {
            var asembly = Assembly.LoadFrom(path);
            if (asembly is null)
                throw new Exception($"Cant load assembly from {path}.");
            return asembly;
        }

    }
}
