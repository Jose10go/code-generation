using CodeGen.Core;

namespace CodeGen.Context
{
    public abstract partial class CodeGenContext<TProject, TRootNode, TProcessEntity> 
    {
        public interface ICommandHandler<TSyntaxNode> : ICommandHandler
            where TSyntaxNode:TRootNode
        {
            void ProcessDocument(TProcessEntity processEntity);
            Command<TSyntaxNode> Command { get; set; }
        }

        //Key Interface
        public interface ICommandHandler<TCommandBuilder,TSyntaxNode> : ICommandHandler<TSyntaxNode>
            where TCommandBuilder : ICommandBuilder<TSyntaxNode>
            where TSyntaxNode : TRootNode
        {
        }
    }
}
