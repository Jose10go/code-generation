using CodeGen.Context;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Editing;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace CodeGen.CSharp.Context
{
    public abstract partial class CSharpContext : CodeGenContext<Project, CSharpSyntaxNode, CompilationUnitSyntax, ISymbol>
    {
        public class CSharpCodeGenerationEngine : ICodeGenerationEngine
        {
            public ICodeGenerationResolver CodeGenerationResolver { get; }
            public Project CurrentProject { get; set; }
            
            public CSharpCodeGenerationEngine(Project project,ICodeGenerationResolver resolver)
            {
                CurrentProject=project;
                CodeGenerationResolver = resolver;
                CodeGenerationResolver.RegisterEngine(this);
                CodeGenerationResolver.BuildContainer();
            }

            public ISingleTarget<CompilationUnitSyntax> SelectNew(string path)
            {
                var filename=Path.GetFileName(path);
                Guid guid = Guid.NewGuid();
                var compilationUnit = SyntaxFactory.CompilationUnit().WithAdditionalAnnotations(new SyntaxAnnotation(guid.ToString()));
                var result = new CSharpSingleTarget<CompilationUnitSyntax>(this,guid,path);
                var document = CurrentProject.AddDocument(filename, compilationUnit, filePath: path);
                CurrentProject = document.Project;
                return result;
            }

            public IMultipleTarget<TSyntaxNode> Select<TSyntaxNode>()
                where TSyntaxNode : CSharpSyntaxNode
            {
                var allTargets = Enumerable.Empty<CSharpSingleTarget<TSyntaxNode>>();
                foreach (var id in CurrentProject.DocumentIds)
                {
                    var doc = CurrentProject.GetDocument(id);
                    var editor = DocumentEditor.CreateAsync(doc).Result;
                    var root = editor.OriginalRoot as CSharpSyntaxNode;
                    var selector = new SelectSyntaxRewriter<TSyntaxNode>(this, doc.FilePath);
                    var newRoot = selector.VisitRoot(root);
                    allTargets = allTargets.Concat(selector.Result);
                    editor.ReplaceNode(root, newRoot);
                    this.CurrentProject = editor.GetChangedDocument().Project;
                }
                return new MultipleTarget<TSyntaxNode>(allTargets);
            }

            public IMultipleTarget<TSyntaxNode0, TSyntaxNode1> Select<TSyntaxNode0,TSyntaxNode1>()
                where TSyntaxNode0 : CSharpSyntaxNode
                where TSyntaxNode1 : CSharpSyntaxNode
            {
                var allTargets = Enumerable.Empty<CSharpSingleTarget<TSyntaxNode0, TSyntaxNode1>>();
                foreach (var id in CurrentProject.DocumentIds)
                {
                    var doc = CurrentProject.GetDocument(id);
                    var editor = DocumentEditor.CreateAsync(doc).Result;
                    var root = editor.OriginalRoot as CSharpSyntaxNode;
                    var selector = new SelectSyntaxRewriter<TSyntaxNode0, TSyntaxNode1>(this, doc.FilePath);
                    var newRoot = selector.VisitRoot(root);
                    allTargets = allTargets.Concat(selector.Result);
                    editor.ReplaceNode(root, newRoot);
                    this.CurrentProject = editor.GetChangedDocument().Project;
                }
                return new MultipleTarget<TSyntaxNode0, TSyntaxNode1>(allTargets);
            }

            public IMultipleTarget<TSyntaxNode0, TSyntaxNode1,TSyntaxNode2> Select<TSyntaxNode0, TSyntaxNode1, TSyntaxNode2>()
                where TSyntaxNode0 : CSharpSyntaxNode
                where TSyntaxNode1 : CSharpSyntaxNode
                where TSyntaxNode2 : CSharpSyntaxNode
            {
                var allTargets = Enumerable.Empty<CSharpSingleTarget<TSyntaxNode0, TSyntaxNode1, TSyntaxNode2>>();
                foreach (var id in CurrentProject.DocumentIds)
                {
                    var doc = CurrentProject.GetDocument(id);
                    var editor = DocumentEditor.CreateAsync(doc).Result;
                    var root = editor.OriginalRoot as CSharpSyntaxNode;
                    var selector = new SelectSyntaxRewriter<TSyntaxNode0, TSyntaxNode1, TSyntaxNode2>(this, doc.FilePath);
                    var newRoot=selector.VisitRoot(root);
                    allTargets = allTargets.Concat(selector.Result);
                    editor.ReplaceNode(root, newRoot);
                    this.CurrentProject = editor.GetChangedDocument().Project;
                }
                return new MultipleTarget<TSyntaxNode0, TSyntaxNode1,TSyntaxNode2>(allTargets);
            }

            public IEnumerable<string> GetAllDocumentsPaths() 
            {
                return CurrentProject.DocumentIds.Select(id => CurrentProject.GetDocument(id).FilePath);
            }

            public CompilationUnitSyntax GetRootNode(string Path)
            {
                return CurrentProject.DocumentIds.Select(id => CurrentProject.GetDocument(id))
                                                 .First(doc=>doc.FilePath==Path)
                                                 .GetSyntaxRootAsync()
                                                 .Result as CompilationUnitSyntax;//TODO: make async???
            }

        }
    }
}