using CodeGen.Core;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Editing;
using System.Collections.Generic;
using System.IO;

namespace CodeGen.CSharp.Context.DocumentEdit
{
    public partial class CSharpContextDocumentEditor : CSharpContext<DocumentEditor>
    {
        public class DocumentEditingCodeGenerationEngine : ICodeGenerationEngine
        {
            private readonly ICodeGenerationResolver Resolver;
            private readonly List<ICommand> CommandsToApply;
            public Project CurrentProject { get; private set; }
            
            public DocumentEditingCodeGenerationEngine(Project project,ICodeGenerationResolver resolver)
            {
                CommandsToApply = new List<ICommand>();
                CurrentProject=project;
                Resolver = resolver;
                Resolver.RegisterEngine(this);
                Resolver.BuildContainer();
            }

            public ITarget<TSyntaxNode> Select<TSyntaxNode>()
                where TSyntaxNode:CSharpSyntaxNode
            {
                var result = new CSharpTarget<TSyntaxNode>(this);
                return result;
            }

            public TCommandBuilder Execute<TCommandBuilder,TNode>(ITarget<TNode> target)
                where TCommandBuilder : ICommand<TNode>
                where TNode : CSharpSyntaxNode
            {
                var command = Resolver.ResolveCommandBuilder<TCommandBuilder, TNode>();
                command.Target = (Target<TNode>)target;
                this.CommandsToApply.Add(command);
                return command;
            }

            public void ApplyChanges()
            {
                foreach (var documentid in CurrentProject.DocumentIds)
                {
                    var document = CurrentProject.GetDocument(documentid);
                    var documentEditor = DocumentEditor.CreateAsync(document).Result;////TODO: make async???
                    foreach (var cmd in CommandsToApply)
                    {
                        var handler = Resolver.ResolveCommandHandler(cmd);
                        if (handler.ProcessDocument(documentEditor))
                        {
                            document = documentEditor.GetChangedDocument();
                            CurrentProject = document.Project;
                        }
                    }
                }
            }

            public ITarget<CompilationUnitSyntax> SelectNew(string path)
            {
                var filename=Path.GetFileName(path);
                var annotation = new SyntaxAnnotation();
                var compilationUnit = SyntaxFactory.CompilationUnit().WithAdditionalAnnotations(annotation);
                CurrentProject = CurrentProject.AddDocument(filename,compilationUnit, filePath: "path").Project;
                ITarget<CompilationUnitSyntax> result = new CSharpTarget<CompilationUnitSyntax>(this);
                result.Where(x => x.HasAnnotation(annotation));
                return result;
            }

        }
    }
}