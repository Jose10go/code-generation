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
                    var editor=DocumentEditor.CreateAsync(doc).Result;
                    var root = editor.OriginalRoot;
                    var targets = root.DescendantNodes()
                                   .OfType<TSyntaxNode>()
                                   .Select(node => new { Node = node, Target = new CSharpSingleTarget<TSyntaxNode>(this, Guid.NewGuid(), doc.FilePath) })
                                   .ToList();//important because if is used as ienumerable then every time its iterated produces targets with diferent ids
                    
                    foreach (var item in targets)
                        editor.ReplaceNode(item.Node, 
                                           item.Node.WithAdditionalAnnotations(new SyntaxAnnotation($"{item.Target.Id}")));
                    this.CurrentProject=editor.GetChangedDocument().Project;
                    allTargets=allTargets.Concat(targets.Select(x=>x.Target));
                }
                return new MultipleTarget<TSyntaxNode>(allTargets);
            }

            public IMultipleTarget<TSyntaxNode0, TSyntaxNode1> Select<TSyntaxNode0,TSyntaxNode1>()
                where TSyntaxNode0 : CSharpSyntaxNode
                where TSyntaxNode1 : CSharpSyntaxNode
            {
                var allTargets = Enumerable.Empty<CSharpSingleTarget<TSyntaxNode0,TSyntaxNode1>>();
                foreach (var id in CurrentProject.DocumentIds)
                {
                    var doc = CurrentProject.GetDocument(id);
                    var editor = DocumentEditor.CreateAsync(doc).Result;
                    var root = editor.OriginalRoot;
                    var targets = root.DescendantNodes()
                                       .OfType<TSyntaxNode1>()
                                       .Select(parentNode => new { Node=parentNode,Target=new CSharpSingleTarget<TSyntaxNode1>(this, Guid.NewGuid(), doc.FilePath) })
                                       .SelectMany(parent => parent.Node
                                                            .DescendantNodes()
                                                            .OfType<TSyntaxNode0>()
                                                            .Select(node => new {Node=node,ParentNode=parent.Node,Target=new CSharpSingleTarget<TSyntaxNode0, TSyntaxNode1>(this, Guid.NewGuid(), parent.Target, doc.FilePath) }))
                                                            .ToList();
                    foreach (var item in targets)
                    {
                        var newNode = item.Node.WithAdditionalAnnotations(new SyntaxAnnotation($"{item.Target.Id}"));
                        var newParent=item.ParentNode.ReplaceNode(item.Node,newNode)
                                                     .WithAdditionalAnnotations(new SyntaxAnnotation($"{item.Target.Parent.Id}"));
                        editor.ReplaceNode(item.ParentNode,newParent);
                    }

                    this.CurrentProject = editor.GetChangedDocument().Project;
                    allTargets = allTargets.Concat(targets.Select(x=>x.Target));
                }
                return new MultipleTarget<TSyntaxNode0,TSyntaxNode1>(allTargets);
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
                    var root = editor.OriginalRoot;
                    var targets = root.DescendantNodes()
                                       .OfType<TSyntaxNode2>()
                                       .Select(node => new { Node = node, Target = new CSharpSingleTarget<TSyntaxNode2>(this, Guid.NewGuid(), doc.FilePath) })
                                       .SelectMany(grandparent => grandparent.Node
                                                                    .DescendantNodes()
                                                                    .OfType<TSyntaxNode1>()
                                                                    .Select(node => new { Node = node, Target = new CSharpSingleTarget<TSyntaxNode1, TSyntaxNode2>(this, Guid.NewGuid(), grandparent.Target, doc.FilePath) })
                                                                    .SelectMany(parent => parent.Node
                                                                                        .DescendantNodes()
                                                                                        .OfType<TSyntaxNode0>()
                                                                                        .Select(node => new { Node = node, ParentNode = parent.Node, GrandparentNode = grandparent.Node, Target = new CSharpSingleTarget<TSyntaxNode0, TSyntaxNode1, TSyntaxNode2>(this, Guid.NewGuid(), parent.Target, doc.FilePath)})))
                                       .ToList();
                    
                    foreach (var item in targets)
                    {
                        var newNode = item.Node.WithAdditionalAnnotations(new SyntaxAnnotation($"{item.Target.Id}"));
                        var newParent = item.ParentNode.ReplaceNode(item.Node,newNode)
                                                         .WithAdditionalAnnotations(new SyntaxAnnotation($"{item.Target.Parent.Id}"));
                        var newGrand = item.GrandparentNode.ReplaceNode(item.ParentNode,newParent)
                                                           .WithAdditionalAnnotations(new SyntaxAnnotation($"{item.Target.Grandparent.Id}"));
                        editor.ReplaceNode(item.GrandparentNode,newGrand);
                    }

                    this.CurrentProject = editor.GetChangedDocument().Project;
                    allTargets = allTargets.Concat(targets.Select(x=>x.Target));
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