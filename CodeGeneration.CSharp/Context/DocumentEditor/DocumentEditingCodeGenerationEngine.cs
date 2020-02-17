using CodeGen.Core;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.Editing;
using System.Collections.Generic;
namespace CodeGen.CSharp.Context.DocumentEdit
{
    public partial class CSharpContextDocumentEditor : CSharpContext<DocumentEditor>
    {
        public class DocumentEditingCodeGenerationEngine : ICodeGenerationEngine
        {
            public IList<Document> ModifiedDocuments { get; private set; }
            private ICodeGenerationResolver Resolver;
            private List<ICommand> CommandsToApply;

            public Project CurrentProject { get; private set; }
            
            public DocumentEditingCodeGenerationEngine(Project project,ICodeGenerationResolver resolver)
            {
                ModifiedDocuments = new List<Document>();
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
                            ModifiedDocuments.Add(document);
                            CurrentProject = document.Project;
                        }
                    }
                }
            }
        }
    }
}