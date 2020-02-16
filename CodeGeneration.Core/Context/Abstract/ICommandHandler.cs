using CodeGen.Core;

namespace CodeGen.Context
{
    public abstract partial class CodeGenContext<TProject, TRootNode,TSemanticModel, TProcessEntity> 
    {
        public interface ICommandHandler<TSyntaxNode> : ICommandHandler
            where TSyntaxNode:TRootNode
        {
            bool ProcessDocument(TProcessEntity processEntity);
        }

        //keyInterface
        public interface ICommandHandler<TCommandBuilder,TSyntaxNode> : ICommandHandler<TSyntaxNode>
            where TCommandBuilder:ICommandBuilder<TSyntaxNode>
            where TSyntaxNode : TRootNode
        {
        }
    }
}
