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
                    var nodes = root.DescendantNodes()
                                       .OfType<TSyntaxNode0>()
                                       .GroupBy(x => x.Ancestors().OfType<TSyntaxNode1>().FirstOrDefault())
                                       .Where(x => x.Key != null)
                                       .ToList();

                    foreach (var parentGroup in nodes)
                    {
                        var parentTarget = new CSharpSingleTarget<TSyntaxNode1>(this, Guid.NewGuid(), doc.FilePath);
                        var parentNode = parentGroup.Key.WithAdditionalAnnotations(new SyntaxAnnotation($"{parentTarget.Id}"));
                        foreach (var node in parentGroup) 
                        {
                            var target = new CSharpSingleTarget<TSyntaxNode0, TSyntaxNode1>(this, Guid.NewGuid(),parentTarget,doc.FilePath);
                            allTargets=allTargets.Append(target);
                            parentNode = parentNode.ReplaceNode(node, node.WithAdditionalAnnotations(new SyntaxAnnotation($"{target.Id}")));
                        }
                        editor.ReplaceNode(parentGroup.Key,parentNode);
                    }

                    this.CurrentProject = editor.GetChangedDocument().Project;
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
                    var nodes = root.DescendantNodes()
                                       .OfType<TSyntaxNode0>()
                                       .GroupBy(x => x.Ancestors().OfType<TSyntaxNode1>().FirstOrDefault())
                                       .Where(x => x.Key != null)
                                       .GroupBy(x => x.Key.Ancestors().OfType<TSyntaxNode2>().FirstOrDefault())
                                       .Where(x => x.Key != null)
                                       .ToList();

                    foreach (var groupGrandparent in nodes)
                    {
                        var grandparentTarget = new CSharpSingleTarget<TSyntaxNode2>(this, Guid.NewGuid(), doc.FilePath);
                        var grandparentNode = groupGrandparent.Key.WithAdditionalAnnotations(new SyntaxAnnotation($"{grandparentTarget.Id}"));
                        foreach (var parentGroup in groupGrandparent)
                        {
                            var parentTarget = new CSharpSingleTarget<TSyntaxNode1,TSyntaxNode2>(this, Guid.NewGuid(),grandparentTarget, doc.FilePath);
                            var parentNode = parentGroup.Key.WithAdditionalAnnotations(new SyntaxAnnotation($"{parentTarget.Id}"));
                            foreach (var node in parentGroup)
                            {
                                var target = new CSharpSingleTarget<TSyntaxNode0, TSyntaxNode1,TSyntaxNode2>(this, Guid.NewGuid(), parentTarget, doc.FilePath);
                                allTargets = allTargets.Append(target);
                                parentNode = parentNode.ReplaceNode(node, node.WithAdditionalAnnotations(new SyntaxAnnotation($"{target.Id}")));
                            }
                            grandparentNode=grandparentNode.ReplaceNode(parentGroup.Key, parentNode);
                        }
                        editor.ReplaceNode(groupGrandparent.Key, grandparentNode);
                    }
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