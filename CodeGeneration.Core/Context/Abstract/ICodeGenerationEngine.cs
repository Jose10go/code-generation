namespace CodeGen.Context
{
    public abstract partial class CodeGenContext<TProject, TRootNode, TSemanticModel, TProcessEntity> 
    {
        public interface ICodeGenerationEngine:Core.ICodeGenerationEngine
        {
            TProject CurrentProject { get; }

            ITarget<TSyntaxNode> Select<TSyntaxNode>() where TSyntaxNode : TRootNode;

            void ApplyChanges();
           
            TCommand Execute<TCommand,TNode>(ITarget<TNode> target) 
                where TCommand : ICommand<TNode>
                where TNode:TRootNode;
        }
    }
}
