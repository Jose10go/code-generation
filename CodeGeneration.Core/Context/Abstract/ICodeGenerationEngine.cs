namespace CodeGen.Context
{
    public abstract partial class CodeGenContext<TProject, TRootNode, TSemanticModel, TProcessEntity> 
    {
        public interface ICodeGenerationEngine:Core.ICodeGenerationEngine
        {
            TProject CurrentSolution { get; }

            ChainTargetBuilder<TSyntaxNode> Select<TSyntaxNode>() where TSyntaxNode : TRootNode;

            void ApplyChanges<TSyntaxNode>(ICommandHandler<TSyntaxNode> commandHandler)
                where TSyntaxNode:TRootNode;
        }
    }
}
