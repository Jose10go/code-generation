using static CodeGen.Context.CodeGenTypelessContext;

namespace CodeGen.Context
{

    public partial class CodeGenContext<TProject, TRootNode, TProcessEntity> 
    {
        public interface ICommandHandler<TCommand,TTarget, TNode> : ICommandHandler
            where TCommand : ICommand<TNode>
            where TTarget : ITarget<TNode>
        {
            new TTarget Target { get; set; }

            new TCommand Command { get; set; }

            TProcessEntity ProcessDocument(TProcessEntity entity);

        }
    }
}
