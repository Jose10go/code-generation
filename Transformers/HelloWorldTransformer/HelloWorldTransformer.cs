using CodeGen.CSharp.Context.DocumentEdit;
using System;

namespace HelloWorldTransformer
{
    public class HelloWorldTransformer : CodeGeneration.CSharp.CodeGenerationTransformer
    {
        public override void Transform()
        {
            Engine.SelectNew("Program.cs")
                  .Execute<CSharpContextDocumentEditor.ICreateNamespace>()
                    .WithName("HelloWorld")
                    .Execute<CSharpContextDocumentEditor.ICreateClass>()
                      .WithName("Program")
                      .MakeStatic()
                      .Execute<CSharpContextDocumentEditor.ICreateMethod>()
                        .WithName("Main")
                        .MakeStatic()
                        .WithBody((dynamic @this) => { Console.WriteLine("Hello World!!!"); });
            
            Engine.ApplyChanges();
        }
    }
}
