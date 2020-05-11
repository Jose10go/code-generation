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
        private readonly List<ITaskItem> _addedNoneFiles = new List<ITaskItem>();
        private readonly List<ITaskItem> _addedCompileFiles = new List<ITaskItem>();
        private readonly List<ITaskItem> _addedContentFiles = new List<ITaskItem>();
        private readonly List<ITaskItem> _addedEmbeddedResourceFiles = new List<ITaskItem>();
        private readonly List<ITaskItem> _removedNoneFiles = new List<ITaskItem>();
        private readonly List<ITaskItem> _removedCompileFiles = new List<ITaskItem>();
        private readonly List<ITaskItem> _removedContentFiles = new List<ITaskItem>();
        private readonly List<ITaskItem> _removedEmbeddedResourceFiles = new List<ITaskItem>();

        [Required]
        public string ProjectFilePath { get; set; }

        [Required]
        public ITaskItem[] TransformerAssemblies { get; set; }

        [Output]
        public ITaskItem[] AddedNoneFiles => _addedNoneFiles.ToArray();

        [Output]
        public ITaskItem[] AddedCompileFiles => _addedCompileFiles.ToArray();

        [Output]
        public ITaskItem[] AddedContentFiles => _addedContentFiles.ToArray();

        [Output]
        public ITaskItem[] AddedEmbeddedResourceFiles => _addedEmbeddedResourceFiles.ToArray();

        [Output]
        public ITaskItem[] RemovedNoneFiles => _removedNoneFiles.ToArray();

        [Output]
        public ITaskItem[] RemovedCompileFiles => _removedCompileFiles.ToArray();

        [Output]
        public ITaskItem[] RemovedContentFiles => _removedContentFiles.ToArray();

        [Output]
        public ITaskItem[] RemovedEmbeddedResourceFiles => _removedEmbeddedResourceFiles.ToArray();

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

            using (var process = new Process())
            {
                process.StartInfo.FileName = $"dotnet";
                process.StartInfo.Arguments = "CodeGeneration.CSharp.Precompilation.dll" + commandLineArgs;
                process.StartInfo.CreateNoWindow = true;
                process.StartInfo.UseShellExecute = false;
                process.StartInfo.WorkingDirectory = Path.GetDirectoryName(typeof(PrecompilationTask).Assembly.Location);
                process.StartInfo.RedirectStandardOutput = true;
                process.StartInfo.RedirectStandardError = true;
                StringBuilder output = new StringBuilder();
                StringBuilder error = new StringBuilder();
                process.OutputDataReceived += (s, e) =>
                {
                    if (!string.IsNullOrEmpty(e.Data))
                        output.Append(e.Data);
                };
                process.ErrorDataReceived += (s, e) =>
                {
                    if (!string.IsNullOrEmpty(e.Data))
                        error.Append(e.Data);
                };
                process.Start();
                process.BeginOutputReadLine();
                process.BeginErrorReadLine();
                process.WaitForExit();

                var error_str = error.ToString();
                if (!string.IsNullOrEmpty(error_str))
                    Log.LogError(error_str);
                ParseAndAnalyzeOutputs(output.ToString());

                if (process.ExitCode == 0)
                    Log.LogMessage("Finished script evaluation");
                else
                    Log.LogError("Got non-zero exit code: " + process.ExitCode);
                return process.ExitCode == 0;
            }
        }

        private void ParseAndAnalyzeOutputs(string data)
        {
            var outputs = data.Split(';')
                              .Where(x=>!string.IsNullOrEmpty(x))
                              .Select(x=>TaskData.FromString(x));
            foreach (var output in outputs)
                switch (output.Kind)
                {
                    case "Compile":
                        ParseAndAnalyzeOutput(this._addedCompileFiles,this._removedCompileFiles, output);
                        break;
                    case "None":
                        ParseAndAnalyzeOutput(this._addedNoneFiles,this._removedNoneFiles, output);
                        break;
                    case "Content":
                        ParseAndAnalyzeOutput(this._addedContentFiles,this._removedContentFiles, output);
                        break;
                    case "EmbeddedResource":
                        ParseAndAnalyzeOutput(this._addedEmbeddedResourceFiles,this._removedEmbeddedResourceFiles, output);
                        break;
                    default:
                        Log.LogWarning($"Invalid output Kind on {data}");
                        break;
                }
            }

        private void ParseAndAnalyzeOutput(List<ITaskItem> list,List<ITaskItem> removeList, TaskData taskdata) 
        {
            if (taskdata.Status == "Updated") 
            {
                Log.LogMessage($"Removing {taskdata.FilePath} from {taskdata.Kind}.");
                removeList.Add(new TaskItem(taskdata.FilePath));
            }
            var newPath = Path.Combine("obj", "Transformers", "CSharp", taskdata.FilePath);
            Log.LogMessage($"Adding {newPath} to {taskdata.Kind}.");
            list.Add(new TaskItem(newPath));
        }

    }

    public class TaskData
    {
        public string FilePath { get; set; }
        public string Status { get; set; }
        public string Kind { get; set; }

        public override string ToString()
        {
            return FilePath + "|" + Kind  + "|" + Status;
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