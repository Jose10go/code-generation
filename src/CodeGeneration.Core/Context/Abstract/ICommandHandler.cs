using CodeGen.Core;

namespace CodeGen.Context
{
    public abstract partial class CodeGenContext<TProject, TBaseNode,TRootNode,TSemanticModel, TProcessEntity> 
        where TRootNode:TBaseNode
    {
        //keyInterface
        public interface ICommandHandler<TCommand,TSyntaxNode,TOutputNode> : ICommandHandler
            where TCommand :ICommand<TSyntaxNode,TOutputNode>
            where TSyntaxNode : TBaseNode
            where TOutputNode : TBaseNode
        {
            TCommand Command { get; }
            ISingleTarget<TOutputNode> ProccessTarget(ISingleTarget<TSyntaxNode> target,ICodeGenerationEngine engine);
        }
    }
}
