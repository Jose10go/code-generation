using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using Microsoft.Build.Framework;
using Microsoft.Build.Utilities;
using System.Linq;
using System;
using System.Text;

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
            string commandLineArgs = $" --project {ProjectFilePath} --transformers {string.Join(",",TransformerAssemblies.Select(x=>x.ItemSpec))}";
            Log.LogMessage("commandLineArgs: " + commandLineArgs);
            
            Process process = new Process();
            process.StartInfo.FileName = $"dotnet";
            process.StartInfo.Arguments = "CodeGeneration.CSharp.Precompilation.dll" + commandLineArgs;
            process.StartInfo.CreateNoWindow = true;
            process.StartInfo.UseShellExecute = false;
            process.StartInfo.WorkingDirectory = Path.GetDirectoryName(typeof(PrecompilationTask).Assembly.Location);
            process.StartInfo.RedirectStandardOutput = true;
            process.StartInfo.RedirectStandardError = true;
            StringBuilder output = new StringBuilder();
            StringBuilder error = new StringBuilder();
            process.OutputDataReceived += (s, e) =>{
                if (!string.IsNullOrEmpty(e.Data))
                    output.Append(e.Data);
            };
            process.ErrorDataReceived += (s, e) => {
                if (!string.IsNullOrEmpty(e.Data))
                    error.Append(e.Data);
            };
            process.Start();
            process.BeginOutputReadLine();
            process.BeginErrorReadLine();
            process.WaitForExit();

            var error_str = error.ToString();
            if(!string.IsNullOrEmpty(error_str))
                Log.LogError(error_str);
            ParseAndAnalyzeOutput(output.ToString());

            if (process.ExitCode == 0)
                Log.LogMessage("Finished script evaluation");
            else
                Log.LogError("Got non-zero exit code: " + process.ExitCode);
            return process.ExitCode == 0;
        }

        private void ParseAndAnalyzeOutput(string data)
        {
            var outputs = data.Split(new[] {Environment.NewLine},StringSplitOptions.None)
                              .Select(x=>TaskData.FromString(x));
            foreach (var output in outputs)
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
                    default:
                        Log.LogWarning($"Invalid output Kind on {data}");
                        break;
                }
            }

    }

    [Serializable]
    public class TaskData
    {
        public string FilePath { get; set; }
        public string Status { get; set; }
        public string Kind { get; set; }

        public override string ToString()
        {
            return FilePath + "|" + Kind  + "|" + Status ;
        }

        public static TaskData FromString(string str)
        {
            var strs = str.Split('|');
            if (str.Length == 0)
                return new TaskData() { FilePath = "FilePath", Kind = "Kind", Status = "Status"};
            return new TaskData() { FilePath = strs[0], Kind = strs[1], Status = strs[2] };
        }
    }

}