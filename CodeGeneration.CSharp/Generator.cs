using System.IO;

namespace CodeGeneration.CSharp
{
    public class Generator : CodeGenerationTask
    {
        public override void DoWork()
        {
            var file = Path.Combine("obj", "A.cs");
            Compiles = file;
            File.WriteAllText(file, "namespace something{class A{}}");
        }

    }
}
