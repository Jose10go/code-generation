using System.IO;

namespace HelloWorldTransformer
{
    public class HelloWorldTransformer : CodeGeneration.CSharp.CodeGenerationTransformer
    {
        public override void Transform()
        {
            File.WriteAllText("Hello World.txt", "Hello World!");
        }
    }
}
