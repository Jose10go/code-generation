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
            public ICodeGenerationResolver CodeGenerationResolver { get; }
            public Project CurrentProject { get; private set; }
            
            public DocumentEditingCodeGenerationEngine(Project project,ICodeGenerationResolver resolver)
            {
                CurrentProject=project;
                CodeGenerationResolver = resolver;
                CodeGenerationResolver.RegisterEngine(this);
                CodeGenerationResolver.BuildContainer();
            }

            public SingleTarget<CompilationUnitSyntax> SelectNew(string path)
            {
                var filename=Path.GetFileName(path);
                var compilationUnit = SyntaxFactory.CompilationUnit();
                var document = CurrentProject.AddDocument(filename, compilationUnit, filePath: path);
                var semanticModel = document.GetSemanticModelAsync().Result;
                CurrentProject = document.Project;
                var result = new CSharpSingleTarget<CompilationUnitSyntax>(this,semanticModel,compilationUnit);
                return result;
            }

            public MultipleTarget<TSyntaxNode> Select<TSyntaxNode>()
                where TSyntaxNode : CSharpSyntaxNode
            {
                return new CSharpMultipleTarget<TSyntaxNode>(this);
            }

            public MultipleTarget<TSyntaxNode0, TSyntaxNode1> Select<TSyntaxNode0,TSyntaxNode1>()
                where TSyntaxNode0 : CSharpSyntaxNode
                where TSyntaxNode1 : CSharpSyntaxNode
            {
                return new CSharpMultipleTarget<TSyntaxNode0, TSyntaxNode1>(this);
            }

            public MultipleTarget<TSyntaxNode0, TSyntaxNode1,TSyntaxNode2> Select<TSyntaxNode0, TSyntaxNode1, TSyntaxNode2>()
                where TSyntaxNode0 : CSharpSyntaxNode
                where TSyntaxNode1 : CSharpSyntaxNode
                where TSyntaxNode2 : CSharpSyntaxNode
            {
                return new CSharpMultipleTarget<TSyntaxNode0, TSyntaxNode1, TSyntaxNode2>(this);
            }

            public void UpdateProject(DocumentEditor processEntity)
            {
                this.CurrentProject = processEntity.GetChangedDocument().Project;
            }

            public DocumentEditor GetProccesEntity<TNode>(SingleTarget<TNode> target) where TNode : CSharpSyntaxNode
            {
                var document = CurrentProject.GetDocument(target.Node.SyntaxTree);
                return DocumentEditor.CreateAsync(document).Result;////TODO: make async???
            }
        
        }
    }
}