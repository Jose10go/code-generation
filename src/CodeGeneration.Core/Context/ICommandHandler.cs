using CodeGen.Core;

namespace CodeGen.Context
{
    public abstract partial class CodeGenContext<TProject, TBaseNode,TRootNode,TSemanticModel> 
        where TRootNode:TBaseNode
    {
        //keyInterface
        public interface ICommandHandler<TCommand> : ICommandHandler
            where TCommand :Core.ICommand
        {
            TCommand Command { get; }
            ISingleTarget<TOutputNode> ProccessTarget<TSpecificCommand, TNode, TOutputNode>(ISingleTargeter<TNode> target, ICodeGenerationEngine engine)
                where TSpecificCommand : TCommand, ICommandOn<TNode>, ICommandResult<TOutputNode>
                where TNode : TBaseNode
                where TOutputNode : TBaseNode;
        }
    }
}
