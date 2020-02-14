using Microsoft.Build.Framework;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.MSBuild;
using System;
using System.Resources;
using static CodeGen.CSharp.Context.DocumentEdit.CSharpContextDocumentEditor;

namespace CodeGeneration.CSharp
{
    public abstract class CodeGenerationTask : Microsoft.Build.Utilities.Task
    {
        [Output]
        public string Compiles { get; set; }
        public string SolutionPath { get; set; }
        protected DocumentEditingCodeGenerationEngine Engine { get; set; }

        public CodeGenerationTask():base()
        {
            var workspace = MSBuildWorkspace.Create();
            workspace.WorkspaceFailed += (sender, args) =>
                                            workspace.Diagnostics.Add(args.Diagnostic);

            var solution = workspace.CurrentSolution;
            Engine = new DocumentEditingCodeGenerationEngine(solution);
        }

        public override bool Execute()
        {
            try
            {
                DoWork();
            }
            catch (Exception e)
            {
                Log.LogErrorFromException(e);
                return false;
            }
            return true;
        }

        public abstract void DoWork();

    }
}
