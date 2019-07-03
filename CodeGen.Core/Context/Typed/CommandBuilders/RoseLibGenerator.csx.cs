//TestCSharpFile.csx.cs
using Microsoft.CodeAnalysis;
using Scripty.Core;
using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using CodeGen.Generators;
using System.Collections.Generic;

namespace ScriptContainer
{
    public class RoseLibScriptContainer
    {
        private readonly string _documentStructureFile;
        private readonly string _descriptionFile;
        private readonly ScriptContext _context;

        public RoseLibScriptContainer(ScriptContext context)
        {
            _documentStructureFile = "generated/*.cs";
            _descriptionFile = "generated/Description.txt";
            _context = context;
        }

        public async Task OutputProjectStructure()
        {
            _context.Output[_descriptionFile].BuildAction = Scripty.Core.Output.BuildAction.GenerateOnly;
            //_context.Output[_documentStructureFile].BuildAction = Scripty.Core.Output.BuildAction.Compile;

            _context.Project.Workspace.WorkspaceFailed += Workspace_WorkspaceFailed;            

            var compilation = await _context.Project.Analysis.GetCompilationAsync();

            _context.Output[_descriptionFile].WriteLine($"Diagnostics: {compilation.GetDiagnostics().Count()} \n" +
                $"{compilation.GetDiagnostics().Select(diag => diag.GetMessage()).Aggregate((a,b) => $"{a}\n{b}")}");

            _context.Output[_descriptionFile].WriteLine($"Syntax trees: {compilation.SyntaxTrees.Count()}");

            ////Get NameSpace tp Enumerate / process_descriptionFile
            foreach (var syntaxTree in compilation.SyntaxTrees.Where(st => st.FilePath.EndsWith("Command.cs")))
            {                
                var root = await syntaxTree.GetRootAsync();
                
                _context.Output[_descriptionFile].WriteLine(syntaxTree.FilePath);

                var commandClass = root.DescendantNodes()
                    .OfType<ClassDeclarationSyntax>()
                    .Where(classDecl => classDecl.Identifier.ToString().EndsWith("Command"));

                if (commandClass.Count() > 0)
                {
                    var currentClass = commandClass.First();

                    var builderProps = currentClass.DescendantNodes()
                        .OfType<PropertyDeclarationSyntax>()
                        .Where(prop => prop.AttributeLists.Any(attrList => attrList.Attributes.Any(attr => attr.Name.ToString() == "BuilderProp")));

                    if (builderProps.Count() > 0)
                    {
                        var semantic = compilation.GetSemanticModel(syntaxTree);     
                        var enclosingClassStr = currentClass.Ancestors().OfType<ClassDeclarationSyntax>().First().ToFullString();

                        var propTypesList = new List<PropertyDescription>();

                        foreach (var builderProp in builderProps)
                        {
                            _context.Output[_descriptionFile].WriteLine(builderProp.ToFullString());
                            propTypesList.Add(new PropertyDescription() { Name = builderProp.Identifier.ToString(), Type = builderProp.Type.ToString() });
                        }

                        var commandBuilderGen = new CommandBuilderGenerator() {
                            CommandName = currentClass.Identifier.ToString(),
                            Properties = propTypesList,
                            Namespace = currentClass.FirstAncestorOrSelf<NamespaceDeclarationSyntax>().Name.ToString(),
                            EnclosingClassDeclaration = enclosingClassStr.Substring(0, enclosingClassStr.IndexOf('{'))
                    };
                        var commandBuilder = commandBuilderGen.TransformText();

                        _context.Output[_documentStructureFile.Replace("*", currentClass.Identifier.ToString() + "Builder")].Write(commandBuilder);

                        _context.Output[_descriptionFile].WriteLine(commandBuilder);
                    }                    
                }

                
            }

            //RecurseCompilationSymbol(zAddressNamespaceSymbol, 1);
        }

        private void Workspace_WorkspaceFailed(object sender, WorkspaceDiagnosticEventArgs e)
        {
            _context.Output[_descriptionFile].WriteLine(e.Diagnostic.Message);
        }

        public void RecurseCompilationSymbol(ISymbol compilationSymbol, int level)
        {
            level++;
            if (compilationSymbol is INamespaceOrTypeSymbol)
            {
                WriteDetailLine(level, compilationSymbol.Kind, compilationSymbol.Name, "");
                foreach (var member in ((INamespaceOrTypeSymbol)compilationSymbol).GetMembers())
                {
                    RecurseCompilationSymbol(member, level);
                }
            }
            else if (compilationSymbol is IPropertySymbol)
            {
                WriteDetailLine(level, compilationSymbol.Kind, compilationSymbol.Name,
                    ((IPropertySymbol)compilationSymbol).GetMethod.ReturnType.Name);
            }
            else
            {
                WriteDetailLine(level, compilationSymbol.Kind, compilationSymbol.Name, "");
            }
        }

        private void WriteDetailLine(int level, SymbolKind symbolKind, string symbolName, string symbolReturnType)
        {
            var symbolKindName = Enum.GetName(typeof(SymbolKind), symbolKind);

            _context.Output[_documentStructureFile].WriteLine($"// {GetStringOfSpaces(level)} {symbolKindName} {symbolName} {symbolReturnType}");
        }

        public static string GetStringOfSpaces(int number)
        {
            number = number * 4;
            string s = "";
            for (int i = 0; i < number; i++)
            {
                s = s + " ";
            }
            return s;
        }
    }
}