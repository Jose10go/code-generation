namespace CodeGen.Context
{
    public abstract partial class CodeGenContext<TProject, TBaseNode, TRootNode, TSemanticModel, TProcessEntity> 
        where TRootNode:TBaseNode
    {
        public interface ICodeGenerationEngine:Core.ICodeGenerationEngine
        {
            TProject CurrentProject { get; }

            ITarget<TSyntaxNode> Select<TSyntaxNode>() 
                where TSyntaxNode : TBaseNode;
            ITarget<TSyntaxNode0> Select<TSyntaxNode0, TSyntaxNode1>() 
                where TSyntaxNode0 : TBaseNode
                where TSyntaxNode1 : TBaseNode;
            ITarget<TSyntaxNode0> Select<TSyntaxNode0, TSyntaxNode1,TSyntaxNode2>()
                where TSyntaxNode0 : TBaseNode
                where TSyntaxNode1 : TBaseNode
                where TSyntaxNode2 : TBaseNode;

            ITarget<TRootNode> SelectNew(string path);

            void ApplyChanges();
           
            TCommand Execute<TCommand,TNode>(ITarget<TNode> target) 
                where TCommand : ICommand<TNode>
                where TNode:TBaseNode;
        }
    }
}
