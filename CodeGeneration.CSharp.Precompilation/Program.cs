using CodeGen.CSharp.Context.DocumentEdit;
using Microsoft.Build.Locator;
using Microsoft.CodeAnalysis.MSBuild;
using System;
using System.Collections.Generic;
using System.CommandLine;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using static CodeGen.CSharp.Context.DocumentEdit.CSharpContextDocumentEditor;

namespace CodeGeneration.CSharp.Precompilation
{
    class Program
    {
        static void Main(string project,string[] compiles,string[] transformers)
        {
            var instance = MSBuildLocator.RegisterDefaults();

            var workspace = MSBuildWorkspace.Create();
            workspace.WorkspaceFailed += (sender, args) =>
                                            workspace.Diagnostics.Add(args.Diagnostic);

            var Project = workspace.OpenProjectAsync(project).Result;
            if (Project == null)
                throw new Exception($"Project is null. ({project})");

            var resolver = new CSharpContextDocumentEditor.CSharpAutofacResolver();
            var Engine = new DocumentEditingCodeGenerationEngine(Project);

            foreach (var item in transformers)
            {
                var assembly=LoadAssembly(item);
                var transformer = LoadTransformer(assembly,Engine,compiles);
                transformer.Transform();
            }
        }

        private static CodeGenerationTransformer LoadTransformer(Assembly assembly,DocumentEditingCodeGenerationEngine Engine,string[] compiles)
        {
            var type=assembly.GetTypes().First(x => x.IsSubclassOf(typeof(CodeGenerationTransformer)));//TODO: Why only one transformer by assembly
            if (type is null) 
                throw new Exception($"There is no transformer on assembly {assembly.FullName}.");
            var transformer = Activator.CreateInstance(type);
            if (transformer is null)
                throw new Exception($"Cant get instance of {type.Name}.");

            CodeGenerationTransformer trans = transformer as CodeGenerationTransformer;
            trans.Compiles = compiles;            
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
