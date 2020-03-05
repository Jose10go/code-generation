using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using Microsoft.Build.Framework;
using Microsoft.Build.Utilities;
using Newtonsoft.Json;
namespace CodeGeneration.CSharp.Precompilation
{
    public class PrecompilationTask : Task
    {
        private readonly List<ITaskItem> _noneFiles = new List<ITaskItem>();
        private readonly List<ITaskItem> _compileFiles = new List<ITaskItem>();
        private readonly List<ITaskItem> _contentFiles = new List<ITaskItem>();
        private readonly List<ITaskItem> _embeddedResourceFiles = new List<ITaskItem>();

        [Required]
        public string ProjectFilePath { get; set; }

        [Required]
        public ITaskItem[] TransformerAssemblies { get; set; }

        [Output]
        public ITaskItem[] NoneFiles => _noneFiles.ToArray();

        [Output]
        public ITaskItem[] CompileFiles => _compileFiles.ToArray();

        [Output]
        public ITaskItem[] ContentFiles => _contentFiles.ToArray();

        [Output]
        public ITaskItem[] EmbeddedResourceFiles => _embeddedResourceFiles.ToArray();

        public override bool Execute()
        {
            if (TransformerAssemblies == null || TransformerAssemblies.Length == 0)
                return true;
            if (string.IsNullOrEmpty(ProjectFilePath))
            {
                Log.LogError("A project file is required");
                return false;
            }
            if (!Path.IsPathRooted(ProjectFilePath))
            {
                Log.LogError("The project file path must be absolute");
                return false;
            }

            // Kick off the evaluation process, which must be done in a seperate process space
            // otherwise MSBuild complains when we construct the Roslyn workspace project since
            // it uses MSBuild to figure out what the project contains and MSBuild only supports
            // one build per process
            //https://github.com/dotnet/roslyn/issues/14206 => https://github.com/daveaglick/Scripty/blob/f02ce93c7b3c7a75885497290e6310720b88568f/src/Scripty.MsBuild/ScriptyTask.cs#L67
            Log.LogMessage("Starting out-of-process precompilation task...");
            string commandLineArgs = $" --project {ProjectFilePath} --transformers {TransformerAssemblies}";
            Log.LogMessage("commandLineArgs: " + commandLineArgs);

            Process process = new Process();
            process.StartInfo.FileName = Path.Combine(Path.GetDirectoryName(typeof(PrecompilationTask).Assembly.Location), "dotnet CodeGeneration.CSharp.Precompilation.dll");
            process.StartInfo.Arguments = commandLineArgs;
            process.StartInfo.CreateNoWindow = true;
            process.StartInfo.UseShellExecute = false;
            process.StartInfo.RedirectStandardOutput = true;
            process.StartInfo.RedirectStandardError = true;
            process.OutputDataReceived += (s, e) => ParseAndAnalyzeOutput(e.Data);
            process.ErrorDataReceived += (s, e) => Log.LogError(e.Data);
            process.Start();
            process.BeginOutputReadLine();
            process.BeginErrorReadLine();
            process.WaitForExit();
            if (process.ExitCode == 0)
                Log.LogMessage("Finished script evaluation");
            else
                Log.LogError("Got non-zero exit code: " + process.ExitCode);
            return process.ExitCode == 0;
        }

        private void ParseAndAnalyzeOutput(string data)
        {
            var output = JsonConvert.DeserializeObject<OutputData>(data);
            
            switch (output.Kind)
            {
                case "Compile":
                    this._compileFiles.Add(new TaskItem(output.FilePath));
                        break;
                case "None":
                    this._noneFiles.Add(new TaskItem(output.FilePath));
                    break;
                case "Content":
                    this._contentFiles.Add(new TaskItem(output.FilePath));
                    break;
                case "EmbeddedResource":
                    this._embeddedResourceFiles.Add(new TaskItem(output.FilePath));
                    break;
                //case _: TODO:
                //    Log.LogWarning($"Invalid output Kind on {data}");
                //    break;
            }
        }

    }
}