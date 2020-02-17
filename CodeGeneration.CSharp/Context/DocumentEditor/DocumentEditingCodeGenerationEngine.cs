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
            public Project CurrentProject { get; private set; }
            public IList<Document> ModifiedDocuments { get; private set; }

            public DocumentEditingCodeGenerationEngine(Project Project)
            {
                ModifiedDocuments = new List<Document>();
                CurrentProject=Project;
                Resolver.RegisterEngine(this);
                Resolver.BuildContainer();
            }

            public void ApplyChanges<TSyntaxNode>(ICommandHandler<TSyntaxNode> commandHandler)
                where TSyntaxNode : CSharpSyntaxNode
            {
                foreach (var documentid in CurrentProject.DocumentIds)
                {
                    var document = CurrentProject.GetDocument(documentid);
                    var documentEditor = DocumentEditor.CreateAsync(document).Result;////TODO: make async???
                    if (commandHandler.ProcessDocument(documentEditor))
                    {
                        document = documentEditor.GetChangedDocument();
                        ModifiedDocuments.Add(document);
                        CurrentProject = document.Project;
                    }
                }
            }

            public Target<TSyntaxNode> Select<TSyntaxNode>()where TSyntaxNode:CSharpSyntaxNode
            {
                var result = new CSharpTarget<TSyntaxNode>(this);
                return result;
            }

        }
    }
}