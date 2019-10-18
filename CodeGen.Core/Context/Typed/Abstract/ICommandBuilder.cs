using static CodeGen.Context.CodeGenTypelessContext;

namespace CodeGen.Context
{

    public partial class CodeGenContext<TProject, TRootNode, TProcessEntity> 
    {
        public interface ICommandBuilder<out TCommand, TNode> : ICommandBuilder<TCommand>
            where TCommand : ICommand<TNode>
        {
            new ITarget<TNode> Target { get; set;}
            void Go(TProcessEntity processEntity);
        }
    }
}
