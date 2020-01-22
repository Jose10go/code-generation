namespace CodeGen.Context
{
    public abstract partial class CodeGenContext<TProject, TRootNode, TProcessEntity> 
    {
        public interface ICodeGenerationEngine:Core.ICodeGenerationEngine
        {
            TProject CurrentSolution { get; }

            ChainTargetBuilder<TSyntaxNode> Select<TSyntaxNode>() where TSyntaxNode : TRootNode;

            new void ApplyChanges<TCommandHandler>(TCommandHandler commandHandler)
            where TCommandHandler : ICommandHandler;
        }
    }
}
