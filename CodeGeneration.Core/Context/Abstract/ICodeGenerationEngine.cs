namespace CodeGen.Context
{
    public abstract partial class CodeGenContext<TProject, TBaseNode, TRootNode, TSemanticModel, TProcessEntity> 
        where TRootNode:TBaseNode
    {
        public interface ICodeGenerationEngine:Core.ICodeGenerationEngine
        {
            TProject CurrentProject { get; }

            ITarget<TSyntaxNode> Select<TSyntaxNode>() where TSyntaxNode : TBaseNode;

            ITarget<TRootNode> SelectNew();

            void ApplyChanges();
           
            TCommand Execute<TCommand,TNode>(ITarget<TNode> target) 
                where TCommand : ICommand<TNode>
                where TNode:TBaseNode;
        }
    }
}
