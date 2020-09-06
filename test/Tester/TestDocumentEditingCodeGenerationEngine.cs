using Microsoft.CodeAnalysis;
using System.IO;
using Buildalyzer;
using Buildalyzer.Workspaces;
using static CodeGen.CSharp.Context.CSharpContext;
using System;

namespace Tests
{
    public class TestDocumentEditingCodeGenerationEngine : CSharpCodeGenerationEngine
    {
        public TestDocumentEditingCodeGenerationEngine() : base(GetProject(), new CSharpAutofacResolver())
        {
        }

        private static Project GetProject() {
            var projectPath= Path.GetFullPath(Path.Combine("..", "..", "..", "..", "Examples", "CommandTests", "CommandTests.csproj"));
            AnalyzerManager manager = new AnalyzerManager();
            ProjectAnalyzer analyzer = manager.GetProject(projectPath);
            AdhocWorkspace workspace = new AdhocWorkspace();
            analyzer.AddBinaryLogger("binarylogtests.binlog");
            var project = analyzer.AddToWorkspace(workspace);
            if (project is null || !project.HasDocuments)
                throw new Exception("Error loading project, check binarylogtests.binlog file for errors.");
            return project;
        }
    }
}
