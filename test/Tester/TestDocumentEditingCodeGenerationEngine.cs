using Microsoft.CodeAnalysis;
using System.IO;
using Buildalyzer;
using Buildalyzer.Workspaces;
using static CodeGen.CSharp.Context.CSharpContext;

namespace Tests
{
    public class TestDocumentEditingCodeGenerationEngine : CSharpCodeGenerationEngine
    {
        public static string projectPath = Path.GetFullPath(Path.Combine("..", "..", "..", "..", "Examples", "CommandTests", "CommandTests.csproj"));

        public TestDocumentEditingCodeGenerationEngine() : base(new AnalyzerManager().GetProject(projectPath).AddToWorkspace(new AdhocWorkspace()), new CSharpAutofacResolver())
        {
        }
    }
}
