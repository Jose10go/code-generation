using System;
using CodeGen.CSharp;
using CodeGeneration.CSharp;
using static CodeGen.CSharp.Context.CSharpContext;

namespace HelloWorldTransformer
{
    public class HelloWorldTransformer :CodeGenerationTransformer
    {
        public override void Transform()
        {
            Engine.SelectNew("Program.cs")
                  .Execute((ICreateNamespace cmd ) => 
                      cmd.WithName("HelloWorld"))
                  .Execute((ICreateClass cmd) => 
                      cmd.WithName("Program")
                          .MakeStatic())
                  .Execute((ICreateMethod cmd) => 
                      cmd.WithName("Main")
                         .MakeStatic()
                         .WithBody(new CodeContext()
                            .StartOrContinueWith(
                            (dynamic @this) => { 
                                Console.WriteLine("Hello World!!!"); 
                            })));
            
        }
    }
}
