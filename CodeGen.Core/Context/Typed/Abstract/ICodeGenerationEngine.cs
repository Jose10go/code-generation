namespace CodeGen.Context
{

    public partial class CodeGenContext<TProject, TRootNode, TProcessEntity> 
    {
        public interface ICodeGenerationEngine : CodeGenTypelessContext.ICodeGenerationEngine
        {
            new TProject ApplyChanges();

            new TProject CurrentSolution { get; }

            IChainTargetBuilder<TSyntaxNode> Select<TSyntaxNode>() where TSyntaxNode : TRootNode;
        }
    }
}
