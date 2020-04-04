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
                foreach (var singleTarget in target as Target<TNode>)
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

            public Target<CompilationUnitSyntax> SelectNew(string path)
            {
                var filename=Path.GetFileName(path);
                var annotation = new SyntaxAnnotation();
                var compilationUnit = SyntaxFactory.CompilationUnit().WithAdditionalAnnotations(annotation);
                CurrentProject = CurrentProject.AddDocument(filename,compilationUnit, filePath: "path").Project;
                var result = new CSharpTarget<CompilationUnitSyntax>(this);
                result.Where(x => x.Node.HasAnnotation(annotation));
                return result;
            }

            public Target<TSyntaxNode> Select<TSyntaxNode>()
                where TSyntaxNode : CSharpSyntaxNode
            {
                return new CSharpTarget<TSyntaxNode>(this);
            }

            public Target<TSyntaxNode0> Select<TSyntaxNode0, TSyntaxNode1>()
                where TSyntaxNode0 : CSharpSyntaxNode
                where TSyntaxNode1 : CSharpSyntaxNode
            {
                return new CSharpTarget<TSyntaxNode0, TSyntaxNode1>(this);
            }

            public Target<TSyntaxNode0> Select<TSyntaxNode0, TSyntaxNode1, TSyntaxNode2>()
                where TSyntaxNode0 : CSharpSyntaxNode
                where TSyntaxNode1 : CSharpSyntaxNode
                where TSyntaxNode2 : CSharpSyntaxNode
            {
                return new CSharpTarget<TSyntaxNode0, TSyntaxNode1, TSyntaxNode2>(this);
            }
        }
    }
}