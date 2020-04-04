using CodeGen.CSharp.Context.DocumentEdit;
using System;

namespace HelloWorldTransformer
{
    public class HelloWorldTransformer : CodeGeneration.CSharp.CodeGenerationTransformer
    {
        public override void Transform()
        {
            Engine.SelectNew("Program.cs")
                  .Execute<CSharpContextDocumentEditor.ICreateNamespace>(
                    cmd => cmd.WithName("HelloWorld"))
                  .Execute<CSharpContextDocumentEditor.ICreateClass>(
                    cmd => cmd.WithName("Program")
                            .MakeStatic())
                  .Execute<CSharpContextDocumentEditor.ICreateMethod>(
                    cmd => cmd.WithName("Main")
                            .MakeStatic()
                            .WithBody((dynamic @this) => { Console.WriteLine("Hello World!!!"); }));
            
        }
    }
}
