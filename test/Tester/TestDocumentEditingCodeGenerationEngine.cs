using CodeGen.CSharp.Context.DocumentEdit;
using Microsoft.CodeAnalysis;
using System.IO;
using static CodeGen.CSharp.Context.DocumentEdit.CSharpContextDocumentEditor;
using Buildalyzer;
using Buildalyzer.Workspaces;

namespace Tests
{
    public class TestDocumentEditingCodeGenerationEngine : DocumentEditingCodeGenerationEngine
    {
        public static string projectPath = Path.GetFullPath(Path.Combine("..", "..", "..", "..", "Examples", "CommandTests", "CommandTests.csproj"));

        public TestDocumentEditingCodeGenerationEngine() : base(new AnalyzerManager().GetProject(projectPath).AddToWorkspace(new AdhocWorkspace()), new CSharpContextDocumentEditor.CSharpAutofacResolver())
        {
        }
    }
}
