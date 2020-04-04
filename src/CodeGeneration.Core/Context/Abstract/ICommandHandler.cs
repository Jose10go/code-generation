using CodeGen.Core;

namespace CodeGen.Context
{
    public abstract partial class CodeGenContext<TProject, TBaseNode,TRootNode,TSemanticModel, TProcessEntity> 
        where TRootNode:TBaseNode
    {
        //keyInterface
        public interface ICommandHandler<TCommand,TSyntaxNode> : ICommandHandler
            where TCommand :ICommand<TSyntaxNode>
            where TSyntaxNode : TBaseNode
        {
            void ProccessNode(TSyntaxNode node, TProcessEntity documentEditor);
        }
    }
}
