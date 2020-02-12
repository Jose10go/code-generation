using Microsoft.Build.Framework;
using System.IO;
using System.Linq;
namespace CodeGeneration.CSharp
{
    public class Generator : Microsoft.Build.Utilities.Task
    {
        public override bool Execute()
        {
            var file =Path.Combine("obj", "A.cs");
            Compiles += ";" + file;
            File.WriteAllText(file,"jerigonza");
            return true;
        }

        [Output]
        public string Compiles { get; set; }
    }
}
