using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Editing;
using System.Collections.Generic;
using System.IO;
using System.Linq;

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
                CurrentProject = document.Project;
                var result = new SingleTarget<CompilationUnitSyntax>(this,compilationUnit);
                return result;
            }

            public MultipleTarget<TSyntaxNode> Select<TSyntaxNode>()
                where TSyntaxNode : CSharpSyntaxNode
            {
                return new MultipleTarget<TSyntaxNode>(this);
            }

            public MultipleTarget<TSyntaxNode0, TSyntaxNode1> Select<TSyntaxNode0,TSyntaxNode1>()
                where TSyntaxNode0 : CSharpSyntaxNode
                where TSyntaxNode1 : CSharpSyntaxNode
            {
                return new MultipleTarget<TSyntaxNode0, TSyntaxNode1>(this);
            }

            public MultipleTarget<TSyntaxNode0, TSyntaxNode1,TSyntaxNode2> Select<TSyntaxNode0, TSyntaxNode1, TSyntaxNode2>()
                where TSyntaxNode0 : CSharpSyntaxNode
                where TSyntaxNode1 : CSharpSyntaxNode
                where TSyntaxNode2 : CSharpSyntaxNode
            {
                return new MultipleTarget<TSyntaxNode0, TSyntaxNode1, TSyntaxNode2>(this);
            }

            public void UpdateProject(DocumentEditor processEntity)
            {
                this.CurrentProject = processEntity.GetChangedDocument().Project;
            }

            public DocumentEditor GetProccesEntity<TNode>(ISingleTarget<TNode> target) where TNode : CSharpSyntaxNode
            {
                var document = CurrentProject.GetDocument(target.Node.SyntaxTree);
                return DocumentEditor.CreateAsync(document).Result;////TODO: make async???
            }

            public IEnumerable<CSharpSyntaxNode> GetDescendantNodes(CSharpSyntaxNode node)
            {
                return node.DescendantNodes().Cast<CSharpSyntaxNode>();
            }

            public IEnumerable<CompilationUnitSyntax> GetRootNodes()
            {
                foreach (var documentid in CurrentProject.DocumentIds)
                    yield return CurrentProject.GetDocument(documentid)
                                               .GetSyntaxRootAsync()
                                               .Result as CompilationUnitSyntax;//TODO: make async???
            }

            public ISymbol GetSemantic<TNode>(TNode node) 
                where TNode : CSharpSyntaxNode
            {
                var documentId = CurrentProject.GetDocumentId(node.SyntaxTree);
                var semanticModel=CurrentProject.GetDocument(documentId).GetSemanticModelAsync().Result;//TODO: make async???
                return semanticModel.GetSymbolInfo(node).Symbol;//TODO: return specific SymbolInfo
            }

            public string GetFilePath<TNode>(TNode node)
                where TNode : CSharpSyntaxNode
            {
                return node.SyntaxTree.FilePath;
            }

        }
    }
}