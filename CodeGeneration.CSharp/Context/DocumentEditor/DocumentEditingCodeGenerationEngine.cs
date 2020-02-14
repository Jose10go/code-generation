﻿using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.Editing;

namespace CodeGen.CSharp.Context.DocumentEdit
{
    public partial class CSharpContextDocumentEditor : CSharpContext<DocumentEditor>
    {
        public class DocumentEditingCodeGenerationEngine : ICSharpCodeGenerationEngine
        {
            public Project CurrentProject { get; private set; }

            public DocumentEditingCodeGenerationEngine(Project Project)
            {
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
                    commandHandler.ProcessDocument(documentEditor);
                    document = documentEditor.GetChangedDocument();
                    CurrentProject = document.Project;
                }
            }

            public ChainCSharpTargetBuilder<TSyntaxNode> Select<TSyntaxNode>()where TSyntaxNode :CSharpSyntaxNode
            {
                var result= Resolver.ResolveTargetBuilder<TSyntaxNode>() as ChainCSharpTargetBuilder<TSyntaxNode>;
                return result;
            }

            ChainTargetBuilder<TSyntaxNode> ICodeGenerationEngine.Select<TSyntaxNode>()
            {
                var result= Resolver.ResolveTargetBuilder<TSyntaxNode>();
                return result;
            }

        }
    }
}