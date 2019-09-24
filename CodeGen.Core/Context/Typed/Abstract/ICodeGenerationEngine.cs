namespace CodeGen.Context
{

    public partial class CodeGenContext<TProject, TRootNode, TProcessEntity> 
    {
        public interface ICodeGenerationEngine : CodeGenTypelessContext.ICodeGenerationEngine
        {
            new TProject ApplyChanges();

            new TProject CurrentSolution { get; }

            new ITargetBuilder<TSyntaxNode> Select<TSyntaxNode>() where TSyntaxNode : TRootNode;
        }
    }
}
