using CodeGen.Context;
using CodeGen.Core;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.Editing;

namespace CodeGen.CSharp.Context.DocumentEdit
{
    public partial class CSharpContextDocumentEditor : CSharpContext<DocumentEditor>
    {
        public class DocumentEditingCodeGenerationEngine : ICSharpCodeGenerationEngine
        {
            public Solution CurrentSolution { get; private set; }

            public DocumentEditingCodeGenerationEngine(Solution solution)
            {
                CurrentSolution=solution;
                Resolver.RegisterEngine(this);
                Resolver.BuildContainer();
            }

            public void ApplyChanges<TCommandHandler>(TCommandHandler commandHandler)
                where TCommandHandler:ICommandHandler
            {
                foreach (var projectId in CurrentSolution.ProjectIds)
                {
                    var project = CurrentSolution.GetProject(projectId);
                    foreach (var documentid in project.DocumentIds)
                    {
                        var document = project.GetDocument(documentid);
                        var documentEditor = DocumentEditor.CreateAsync(document).Result;////TODO: make async???
                        commandHandler.ProcessDocument(documentEditor);
                        document = documentEditor.GetChangedDocument();
                        project = document.Project;
                    }
                    CurrentSolution = project.Solution;
                }
            }

            public IChainCSharpTargetBuilder<TSyntaxNode> Select<TSyntaxNode>()where TSyntaxNode :CSharpSyntaxNode
            {
                var result= Resolver.ResolveTargetBuilder<TSyntaxNode>() as IChainCSharpTargetBuilder<TSyntaxNode>;
                result.Engine = this;
                return result;
            }

            IChainTargetBuilder<TSyntaxNode> ICodeGenerationEngine.Select<TSyntaxNode>()
            {
                var result= Resolver.ResolveTargetBuilder<TSyntaxNode>();
                result.Engine = this;
                return result;
            }

            void Core.ICodeGenerationEngine.ApplyChanges<TCommandHandler>(TCommandHandler commandHandler)
            {
                ApplyChanges(commandHandler as ICommandHandler);
            }
        }
    }
}