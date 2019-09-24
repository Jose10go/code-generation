using static CodeGen.Context.CodeGenTypelessContext;

namespace CodeGen.Context
{

    public partial class CodeGenContext<TProject, TRootNode, TProcessEntity> 
    {
        public interface ICommandHandler<TCommand, out TTarget, TNode> : ICommandHandler
            where TCommand : ICommand<TNode>
            where TTarget : ITarget<TNode>
        {
            new TTarget Target { get; }

            new TCommand Command { get; }

            TProcessEntity ProcessDocument(TProcessEntity entity);
        }
    }
}
