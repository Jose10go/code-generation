using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.Editing;

namespace CodeGen.Context.CSharp
{
    public partial class CSharpContext<TProcessEntity> : CodeGenContext<Solution, CSharpSyntaxNode, TProcessEntity>
    {
        public class DocumentEditingCodeGenerationEngine : ICSharpCodeGenerationEngine
        {
            public Solution CurrentSolution { get; private set; }

            object CodeGenTypelessContext.ICodeGenerationEngine.CurrentSolution => CurrentSolution;

            protected SolutionEditor Editor { get; private set; }

            public DocumentEditingCodeGenerationEngine(Solution solution)
            {
                Editor = new SolutionEditor(solution);
                Resolver.RegisterEngine(this);
                Resolver.BuildContainer();
            }

            public Solution ApplyChanges()
            {
                return Editor.GetChangedSolution();
            }

            object CodeGenTypelessContext.ICodeGenerationEngine.ApplyChanges()
            {
                return ApplyChanges();
            }

            public IChainCSharpTargetBuilder<TSyntaxNode> Select<TSyntaxNode>()where TSyntaxNode :CSharpSyntaxNode
            {
                return Resolver.ResolveTargetBuilder<TSyntaxNode>() as IChainCSharpTargetBuilder<TSyntaxNode>;
            }

            IChainTargetBuilder<TSyntaxNode> ICodeGenerationEngine.Select<TSyntaxNode>()
            {
                return Resolver.ResolveTargetBuilder<TSyntaxNode>();
            }

        }
    }
}