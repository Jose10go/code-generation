using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.CSharp;
using Xunit;
using System.IO;
using static CodeGen.CSharp.Context.CSharpContext;
using Xunit.Abstractions;
using System.Linq;
using System;
using System.Collections.Generic;

namespace Tests
{
    public class CommandTests:IClassFixture<TestDocumentEditingCodeGenerationEngine>
    {
        readonly CSharpCodeGenerationEngine engine;
        readonly ITestOutputHelper output;
        string ProjectPath => engine.CurrentProject.FilePath;

        public CommandTests(ITestOutputHelper output,TestDocumentEditingCodeGenerationEngine engine)
        {
            this.output = output;
            this.engine = engine;
        }

        private SyntaxTree ParseFile(string path)
        {
            using (FileStream f = new FileStream(path, FileMode.Open))
            using (StreamReader reader = new StreamReader(f))
            {
                var text = reader.ReadToEnd();
                return SyntaxFactory.ParseSyntaxTree(text);
            }
        }

        class T { }
        [Fact]
        public void CreateClass()
        {
            string inpath = Path.Combine(Path.GetDirectoryName(ProjectPath), "CreateClass", "in.cs");
            string outpath = Path.Combine(Path.GetDirectoryName(ProjectPath), "CreateClass", "out.cs");

            engine.SelectNew(inpath)
                  .Execute((ICreateNamespace cmd) => 
                        cmd.WithName("Tests.Examples.CreateClass")
                           .Using("System")
                           .Using<List<object>>())
                  .Execute((ICreateClass cmd) => cmd.WithName("A")
                                                    .MakeGenericIn<T>()
                                                    .WithConstraints<T>("IEnumerable<T>")
                                                    .InheritsFrom<Dictionary<string,T>>()
                                                    .Implements<ICollection<T>,IComparable<T>>()
                                                    .MakePublic()
                                                    .MakePartial()
                                                    .MakeStatic());

            Document document_in = engine.CurrentProject.Documents.First(x => x.FilePath == inpath);
            engine.CurrentProject.GetDocument(document_in.Id).TryGetSyntaxTree(out var st1);
            var st2 = ParseFile(outpath);
            Assert.True(st1.IsEquivalentTo(st2));
        }

        [Fact]
        public void CreateStruct()
        {
            string inpath = Path.Combine(Path.GetDirectoryName(ProjectPath), "CreateStruct", "in.cs");
            string outpath = Path.Combine(Path.GetDirectoryName(ProjectPath), "CreateStruct", "out.cs");

            engine.SelectNew(inpath)
                  .Execute((ICreateNamespace cmd) =>
                        cmd.WithName("Tests.Examples.CreateStruct")
                           .Using("System")
                           .Using<List<object>>())
                  .Execute((ICreateStruct cmd) => cmd.WithName("A")
                                                     .MakeGenericIn<T>()
                                                     .WithConstraints<T>("IEnumerable<T>")
                                                     .Implements<IDictionary<string, T>,ICollection<T>, IComparable<T>>()
                                                     .MakePublic()
                                                     .MakePartial());

            Document document_in = engine.CurrentProject.Documents.First(x => x.FilePath == inpath);
            engine.CurrentProject.GetDocument(document_in.Id).TryGetSyntaxTree(out var st1);
            var st2 = ParseFile(outpath);
            Assert.True(st1.IsEquivalentTo(st2));
        }

        [Fact]
        public void CreateInterface()
        {
            string inpath = Path.Combine(Path.GetDirectoryName(ProjectPath), "CreateInterface", "in.cs");
            string outpath = Path.Combine(Path.GetDirectoryName(ProjectPath), "CreateInterface", "out.cs");

            var _interface=
            engine.SelectNew(inpath)
                  .Execute((ICreateNamespace cmd) =>
                        cmd.WithName("Tests.Examples.CreateInterface")
                           .Using("System")
                           .Using<List<object>>())
                  .Execute((ICreateInterface cmd) => cmd.WithName("IA")
                                                        .MakeGenericIn<T>()
                                                        .WithConstraints<T>("IEnumerable<string>")
                                                        .Implements<IDictionary<string,T>,ICollection<T>>()
                                                        .MakeInternal()
                                                        .MakePartial());
            _interface
                .Execute((ICreateMethod cmd) =>cmd.WithName("ToString")
                                                  .Returns<string>());
            _interface
               .Execute((ICreateMethod cmd) => cmd.WithName("Parse")
                                                  .Returns("IA<T>"));

            Document document_in = engine.CurrentProject.Documents.First(x => x.FilePath == inpath);
            engine.CurrentProject.GetDocument(document_in.Id).TryGetSyntaxTree(out var st1);
            var st2 = ParseFile(outpath);
            Assert.True(st1.IsEquivalentTo(st2));
        }

        class TGeneric { }
        [Fact]
        public void CloneClass()
        {
            string inpath = Path.Combine(Path.GetDirectoryName(ProjectPath), "CloneClass", "in.cs");
            string outpath = Path.Combine(Path.GetDirectoryName(ProjectPath), "CloneClass", "out.cs");

            engine.Select<ClassDeclarationSyntax>()
                  .Where(x => x.DocumentPath == inpath)
                  .Using(x=>x.Node.Identifier.Text,out var keyName)
                  .Execute((ICloneClass cmd)=>cmd.Get(keyName,out var name)
                                                 .WithName(name + "_generated")
                                                 .Implements<IEnumerable<string>>()
                                                 .MakeGenericIn<TGeneric>()
                                                 .WithConstraints<TGeneric>("Console")
                                                 .MakePublic());

            Document document_in = engine.CurrentProject.Documents.First(x=>x.FilePath==inpath);
            engine.CurrentProject.GetDocument(document_in.Id).TryGetSyntaxTree(out var st1);
            var st2 = ParseFile(outpath);
            Assert.True(st1.IsEquivalentTo(st2));
        }

        [Fact]
        public void ModifyClass()
        {
            string inpath = Path.Combine(Path.GetDirectoryName(ProjectPath), "ModifyClass", "in.cs");
            string outpath = Path.Combine(Path.GetDirectoryName(ProjectPath), "ModifyClass", "out.cs");

            engine.Select<ClassDeclarationSyntax>()
                  .Where(x => x.DocumentPath == inpath)
                  .Using(x => x.Node.Identifier.Text, out var keyName)
                  .Execute((IModifyClass cmd) => cmd.Get(keyName, out var name)
                                                 .WithName(name + "_modified")
                                                 .Implements<ICollection<string>>()
                                                 .MakeGenericIn<TGeneric>()
                                                 .WithStructConstraint<TGeneric>()
                                                 .MakePublic());

            Document document_in = engine.CurrentProject.Documents.First(x => x.FilePath == inpath);
            engine.CurrentProject.GetDocument(document_in.Id).TryGetSyntaxTree(out var st1);
            var st2 = ParseFile(outpath);
            Assert.True(st1.IsEquivalentTo(st2));
        }

        [Fact]
        public void CloneInterface()
        {
            string inpath = Path.Combine(Path.GetDirectoryName(ProjectPath), "CloneInterface", "in.cs");
            string outpath = Path.Combine(Path.GetDirectoryName(ProjectPath), "CloneInterface", "out.cs");

            engine.Select<InterfaceDeclarationSyntax>()
                  .Where(x => x.DocumentPath == inpath)
                  .Using(x => x.Node.Identifier.Text, out var keyName)
                  .Execute((ICloneInterface cmd) => cmd.Get(keyName, out var name)
                                                       .WithName(name + "_generated")
                                                       .MakeGenericIn<T>()
                                                       .WithClassConstraint<T>()
                                                       .MakeInternal())

                  .Execute((ICreateMethod cmd)=>cmd.WithName("goodBye"));

            Document document_in = engine.CurrentProject.Documents.First(x => x.FilePath == inpath);
            engine.CurrentProject.GetDocument(document_in.Id).TryGetSyntaxTree(out var st1);
            var st2 = ParseFile(outpath);
            Assert.True(st1.IsEquivalentTo(st2));
        }

        [Fact]
        public void ModifyInterface()
        {
            string inpath = Path.Combine(Path.GetDirectoryName(ProjectPath), "ModifyInterface", "in.cs");
            string outpath = Path.Combine(Path.GetDirectoryName(ProjectPath), "ModifyInterface", "out.cs");

            engine.Select<InterfaceDeclarationSyntax>()
                  .Where(x => x.DocumentPath == inpath)
                  .Using(x => x.Node.Identifier.Text, out var keyName)
                  .Execute((IModifyInterface cmd) => cmd.Get(keyName, out var name)
                                                       .WithName(name + "_modified")
                                                       .MakeGenericIn<T>()
                                                       .WithClassConstraint<T>()
                                                       .WithNewConstraint<T>()
                                                       .MakeInternal())

                  .Execute((ICreateMethod cmd) => cmd.WithName("goodBye"));

            Document document_in = engine.CurrentProject.Documents.First(x => x.FilePath == inpath);
            engine.CurrentProject.GetDocument(document_in.Id).TryGetSyntaxTree(out var st1);
            var st2 = ParseFile(outpath);
            Assert.True(st1.IsEquivalentTo(st2));
        }

        [Fact]
        public void CloneStruct()
        {
            string inpath = Path.Combine(Path.GetDirectoryName(ProjectPath), "CloneStruct", "in.cs");
            string outpath = Path.Combine(Path.GetDirectoryName(ProjectPath), "CloneStruct", "out.cs");

            engine.Select<StructDeclarationSyntax>()
                  .Where(x => x.DocumentPath == inpath)
                  .Using(x => x.Node.Identifier.Text, out var keyName)
                  .Execute((ICloneStruct cmd) => cmd.Get(keyName, out var name)
                                                 .WithName(name + "_generated")
                                                 .Implements<IEnumerable<string>>()
                                                 .MakeGenericIn<TGeneric>()
                                                 .WithConstraints<TGeneric>("Console")
                                                 .MakeProtected());

            Document document_in = engine.CurrentProject.Documents.First(x => x.FilePath == inpath);
            engine.CurrentProject.GetDocument(document_in.Id).TryGetSyntaxTree(out var st1);
            var st2 = ParseFile(outpath);
            Assert.True(st1.IsEquivalentTo(st2));
        }

        [Fact]
        public void ModifyStruct()
        {
            string inpath = Path.Combine(Path.GetDirectoryName(ProjectPath), "ModifyStruct", "in.cs");
            string outpath = Path.Combine(Path.GetDirectoryName(ProjectPath), "ModifyStruct", "out.cs");

            engine.Select<StructDeclarationSyntax>()
                  .Where(x => x.DocumentPath == inpath)
                  .Using(x => x.Node.Identifier.Text, out var keyName)
                  .Execute((IModifyStruct cmd) => cmd.Get(keyName, out var name)
                                                     .WithName(name + "_modified")
                                                     .Implements<ICollection<double>>()
                                                     .MakeGenericIn<TGeneric>()
                                                     .WithStructConstraint<TGeneric>()
                                                     .MakeProtected());

            Document document_in = engine.CurrentProject.Documents.First(x => x.FilePath == inpath);
            engine.CurrentProject.GetDocument(document_in.Id).TryGetSyntaxTree(out var st1);
            var st2 = ParseFile(outpath);
            Assert.True(st1.IsEquivalentTo(st2));
        }

        [Fact]
        public void CreateProperty()
        {
            string inpath = Path.Combine(Path.GetDirectoryName(ProjectPath), "CreateProperty", "in.cs");
            string outpath = Path.Combine(Path.GetDirectoryName(ProjectPath), "CreateProperty", "out.cs");

            var classTarget = engine.SelectNew(inpath)
                  .Execute((ICreateNamespace cmd) =>
                        cmd.WithName("Tests.Examples.CreateProperty")
                           .Using("System")
                           .Using<List<int>>())
                  .Execute((ICreateClass cmd) => cmd.WithName("A")
                                                    .MakeProtected()
                                                    .MakeAbstract());

            classTarget.Execute((ICreateProperty cmd) => cmd.WithName("SomeString")
                                                            .Returns<string>()
                                                            .WithGet("{return null;}"));

            classTarget.Execute((ICreateProperty cmd) => cmd.WithName("SomeInt")
                                                            .MakeStatic()
                                                            .MakePublic()
                                                            .Returns<int>()
                                                            .MakeSetPrivate()
                                                            .WithGet("{return 100;}")
                                                            .MakeGetInternal());

            Document document_in = engine.CurrentProject.Documents.First(x => x.FilePath == inpath);
            engine.CurrentProject.GetDocument(document_in.Id).TryGetSyntaxTree(out var st1);
            var st2 = ParseFile(outpath);
            Assert.True(st1.IsEquivalentTo(st2));
        }

        [Fact]
        public void CloneProperty()
        {
            string inpath = Path.Combine(Path.GetDirectoryName(ProjectPath), "CloneProperty", "in.cs");
            string outpath = Path.Combine(Path.GetDirectoryName(ProjectPath), "CloneProperty", "out.cs");

            var propTarget = engine.Select<PropertyDeclarationSyntax>()
                                   .Where(x => x.DocumentPath == inpath);
            propTarget
                  .Execute((ICloneProperty cmd) => cmd.WithName("SomeOther")
                                                      .MakeStatic()
                                                      .MakePublic());
            propTarget
                  .Execute((ICloneProperty cmd) => cmd.WithName("SomeOtherOne")
                                                      .MakeProtected()
                                                      .Returns<string>()
                                                      .WithGet("{return \"hola\";}")
                                                      .MakeGetPrivate());

            Document document_in = engine.CurrentProject.Documents.First(x => x.FilePath == inpath);
            engine.CurrentProject.GetDocument(document_in.Id).TryGetSyntaxTree(out var st1);
            var st2 = ParseFile(outpath);
            Assert.True(st1.IsEquivalentTo(st2));
        }

        [Fact]
        public void ModifyProperty()
        {
            string inpath = Path.Combine(Path.GetDirectoryName(ProjectPath), "ModifyProperty", "in.cs");
            string outpath = Path.Combine(Path.GetDirectoryName(ProjectPath), "ModifyProperty", "out.cs");

           engine.Select<PropertyDeclarationSyntax>()
                 .Where(x => x.DocumentPath == inpath)
                 .Execute((IModifyProperty cmd) => cmd.WithName("SomeOther")
                                                      .Returns<object>()
                                                      .MakeStatic()
                                                      .MakePrivate());

            Document document_in = engine.CurrentProject.Documents.First(x => x.FilePath == inpath);
            engine.CurrentProject.GetDocument(document_in.Id).TryGetSyntaxTree(out var st1);
            var st2 = ParseFile(outpath);
            Assert.True(st1.IsEquivalentTo(st2));
        }

        [Fact]
        public void CloneMethod()
        {
            string inpath = Path.Combine(Path.GetDirectoryName(ProjectPath), "CloneMethod", "in.cs");
            string outpath = Path.Combine(Path.GetDirectoryName(ProjectPath), "CloneMethod", "out.cs");
            
            engine.Select<MethodDeclarationSyntax>()
                  .Where(x => x.DocumentPath == inpath)
                  .Using(x=>x.Node.Identifier.Text,out var keyName)
                  .Execute((ICloneMethod cmd)=>cmd.Get(keyName,out var name)
                                                  .MakePublic()
                                                  .WithName(name + "_generated")
                                                  .WithBody("{Console.WriteLine(\"hello my friend.\");}"));

            //This Comment is preserved to mark the moment :)
            //.WithBody((dynamic @this)=>{ System.Console.WriteLine("hello my friend.");})//this is the best idea ever...

            Document document_in = engine.CurrentProject.Documents.First(x => x.FilePath == inpath); 
            engine.CurrentProject.GetDocument(document_in.Id).TryGetSyntaxTree(out var st1);
            var st2 = ParseFile(outpath);
            Assert.True(st1.IsEquivalentTo(st2));
        }

        class T1 { }
        class T2 { }
        [Fact]
        public void ExtensionMethod()
        {
            string inpath = Path.Combine(Path.GetDirectoryName(ProjectPath), "ExtensionMethod", "in.cs");
            string outpath = Path.Combine(Path.GetDirectoryName(ProjectPath), "ExtensionMethod", "out.cs");

            engine.SelectNew(inpath)
                  .Execute((ICreateNamespace cmd)=>cmd.WithName("ExtensionMethod")
                                                      .Using("System"))
                  .Execute((ICreateClass cmd)=>cmd.WithName("Extensions")
                                                  .MakePublic()
                                                  .MakeStatic()
                                                  .MakeGenericIn<T1>())
                  .Execute((ICreateMethod cmd) => cmd.MakePublic()
                                                     .MakeStatic()
                                                     .WithName("ExtensionMethod")
                                                     .Returns<T2>()
                                                     .MakeGenericIn<T2>()
                                                        .WithConstraints<T2>("T1")
                                                     .WithParameters<T1>("self")
                                                        .WithThisParameter("self")
                                                     .WithParameters<Func<T1,T2>>("func")
                                                     .WithParameters<string>("message")
                                                        .WithDefaultValue("message","hello")   
                                                     .WithBody(@"{
                                                                    Console.WriteLine(message);
                                                                    return func(self);
                                                                  }"));

            Document document_in = engine.CurrentProject.Documents.First(x => x.FilePath == inpath);
            engine.CurrentProject.GetDocument(document_in.Id).TryGetSyntaxTree(out var st1);
            var st2 = ParseFile(outpath);
            Assert.True(st1.IsEquivalentTo(st2));
        }

        [Fact]
        public void ModifyMethod()
        {
            string inpath = Path.Combine(Path.GetDirectoryName(ProjectPath), "ModifyMethod", "in.cs");
            string outpath = Path.Combine(Path.GetDirectoryName(ProjectPath), "ModifyMethod", "out.cs");

            engine.Select<MethodDeclarationSyntax>()
                  .Where(x => x.DocumentPath == inpath)
                  .Using(x => x.Node.Identifier.Text, out var keyName)
                  .Execute((IModifyMethod cmd) => cmd.Get(keyName, out var name)
                                                     .WithName(name + "_modified")
                                                     .MakePublic()
                                                     .WithBody("{Console.WriteLine(\"hello my friend.\");}"));

          
            Document document_in = engine.CurrentProject.Documents.First(x => x.FilePath == inpath);
            engine.CurrentProject.GetDocument(document_in.Id).TryGetSyntaxTree(out var st1);
            var st2 = ParseFile(outpath);
            Assert.True(st1.IsEquivalentTo(st2));
        }

        [Fact]
        public void CreateMultipleMethods()
        {
            string inpath = Path.Combine(Path.GetDirectoryName(ProjectPath), "CreateMultipleMethods", "in.cs");
            string outpath = Path.Combine(Path.GetDirectoryName(ProjectPath), "CreateMultipleMethods", "out.cs");

            var _class = engine.SelectNew(inpath)
                               .Execute((ICreateNamespace cmd) => cmd.WithName("MultipleMethods"))
                               .Execute((ICreateClass cmd) => cmd.WithName("A")
                                                                 .MakePublic());
            for (int i = 0; i < 99; i++)
            {
                _class.Execute((ICreateMethod cmd) => cmd.WithName("returns" + i)
                                                         .WithAttributes("GeneratedCode")
                                                         .Returns<int>()
                                                         .MakeStatic()
                                                         .WithBody($"{{return {i};}}"));
            } 

            Document document_in = engine.CurrentProject.Documents.First(x => x.FilePath == inpath);
            engine.CurrentProject.GetDocument(document_in.Id).TryGetSyntaxTree(out var st1);
            var st2 = ParseFile(outpath);
            Assert.True(st1.IsEquivalentTo(st2));
        }

        [Fact]
        public void ReplaceInvocation()
        {
            string inpath = Path.Combine(Path.GetDirectoryName(ProjectPath), "ReplaceInvocation", "in.cs");
            string outpath = Path.Combine(Path.GetDirectoryName(ProjectPath), "ReplaceInvocation", "out.cs");

            engine.Select<ParenthesizedLambdaExpressionSyntax, ArgumentSyntax, InvocationExpressionSyntax>()
                  .Where(x => { 
                      var s = (IMethodSymbol)x.Grandparent.SemanticSymbol;
                      if (s is null)
                          return false;
                      return s.Name == "f" && s.ContainingType.Name == "A";})
                  .Where(x => x.DocumentPath == inpath)
                  .Using(target => target.Node.Body.ToString().Replace("@this", "this"), out var bodyKey)
                  .Execute((IReplaceExpression<ParenthesizedLambdaExpressionSyntax> cmd) => cmd.Get(bodyKey, out var stringBody)
                                                                                               .With(SyntaxFactory.ParseExpression($"\"{stringBody}\"")));
                  

            Document document_in = engine.CurrentProject.Documents.First(x => x.FilePath == inpath);
            engine.CurrentProject.GetDocument(document_in.Id).TryGetSyntaxTree(out var st1);
            var st2 = ParseFile(outpath);
            Assert.True(st1.IsEquivalentTo(st2));
        }

        [Fact] 
        public void HelloWorldTest() 
        {
            string outpath = Path.Combine(Path.GetDirectoryName(ProjectPath), "HelloWorld", "out.cs");
            string inpath = Path.Combine(Path.GetDirectoryName(ProjectPath), "HelloWorld", "in.cs");

            engine.SelectNew(inpath)
                  .Execute((ICreateNamespace cmd) => cmd.WithName("HelloWorld"))
                  .Execute((ICreateClass cmd) => cmd.WithName("Program")
                                                    .MakeStatic())
                  .Execute((ICreateMethod cmd) => cmd.WithName("Main")
                                                     .MakeStatic()
                                                     .WithBody("{System.Console.WriteLine(\"Hello World!!!\"); }"));

            Document document_in = engine.CurrentProject.Documents.First(x => x.FilePath == inpath);
            engine.CurrentProject.GetDocument(document_in.Id).TryGetSyntaxTree(out var st1);
            var st2 = ParseFile(outpath);
            Assert.True(st1.IsEquivalentTo(st2));
        }

    }
}
