using CodeGen.Core;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Editing;
using System;
using System.Collections.Generic;
using System.IO;

namespace CodeGen.CSharp.Context.DocumentEdit
{
    public partial class CSharpContextDocumentEditor : CSharpContext<DocumentEditor>
    {
        public class DocumentEditingCodeGenerationEngine : ICodeGenerationEngine
        {
            private readonly ICodeGenerationResolver Resolver;
            public Project CurrentProject { get; private set; }
            
            public DocumentEditingCodeGenerationEngine(Project project,ICodeGenerationResolver resolver)
            {
                CurrentProject=project;
                Resolver = resolver;
                Resolver.RegisterEngine(this);
                Resolver.BuildContainer();
            }

            public TCommand Execute<TCommand,TNode>(ITarget<TNode> target,Func<TCommand, TCommand> commandModifiers)
                where TCommand : ICommand<TNode>
                where TNode : CSharpSyntaxNode
            {
                var command = Resolver.ResolveCommandBuilder<TCommand, TNode>();
                command.Target = target;
                command = commandModifiers(command);
                ApplyChanges(command,command.Target);
                return command;
            }

            private void ApplyChanges<TCommand,TNode>(TCommand command,ITarget<TNode> target)
                where TCommand : ICommand<TNode>
                where TNode : CSharpSyntaxNode
            {
                var handler = Resolver.ResolveCommandHandler<TCommand,TNode>(command);
                if (target is ISingleTarget<TNode>) 
                {
                    var singleTarget = target as ISingleTarget<TNode>;
                    var documentId = CurrentProject.GetDocumentId(singleTarget.Node.SyntaxTree);
                    var document = CurrentProject.GetDocument(documentId);
                    var documentEditor = DocumentEditor.CreateAsync(document).Result;////TODO: make async???
                    handler.ProccessNode(singleTarget.Node, documentEditor);
                    document = documentEditor.GetChangedDocument();
                    CurrentProject = document.Project;
                    return;
                }

                Dictionary<DocumentId,DocumentEditor> memoized=new Dictionary<DocumentId, DocumentEditor>();
                foreach (var singleTarget in target as CSharpMultipleTarget<TNode>)
                {
                    var documentId = CurrentProject.GetDocumentId(singleTarget.Node.SyntaxTree);
                    var document = CurrentProject.GetDocument(documentId);
                    DocumentEditor documentEditor;
                    if (!memoized.ContainsKey(documentId))
                        memoized.Add(documentId, DocumentEditor.CreateAsync(document).Result);////TODO: make async???
                    documentEditor = memoized[documentId];
                    handler.ProccessNode(singleTarget.Node, documentEditor);
                }

                foreach (var item in memoized)
                    CurrentProject = item.Value.GetChangedDocument().Project;
            }

            public CSharpSingleTarget<CompilationUnitSyntax> SelectNew(string path)
            {
                var filename=Path.GetFileName(path);
                var compilationUnit = SyntaxFactory.CompilationUnit();
                var document = CurrentProject.AddDocument(filename, compilationUnit, filePath: path);
                var semanticModel = document.GetSemanticModelAsync().Result;
                CurrentProject = document.Project;
                var result = new CSharpSingleTarget<CompilationUnitSyntax>(this,semanticModel,compilationUnit);
                return result;
            }

            public CSharpMultipleTarget<TSyntaxNode> Select<TSyntaxNode>()
                where TSyntaxNode : CSharpSyntaxNode
            {
                return new CSharpMultipleTarget<TSyntaxNode>(this);
            }

            public CSharpMultipleTarget<TSyntaxNode0, TSyntaxNode1> Select<TSyntaxNode0,TSyntaxNode1>()
                where TSyntaxNode0 : CSharpSyntaxNode
                where TSyntaxNode1 : CSharpSyntaxNode
            {
                return new CSharpMultipleTarget<TSyntaxNode0, TSyntaxNode1>(this);
            }

            public CSharpMultipleTarget<TSyntaxNode0, TSyntaxNode1,TSyntaxNode2> Select<TSyntaxNode0, TSyntaxNode1, TSyntaxNode2>()
                where TSyntaxNode0 : CSharpSyntaxNode
                where TSyntaxNode1 : CSharpSyntaxNode
                where TSyntaxNode2 : CSharpSyntaxNode
            {
                return new CSharpMultipleTarget<TSyntaxNode0, TSyntaxNode1, TSyntaxNode2>(this);
            }
        }
    }
}