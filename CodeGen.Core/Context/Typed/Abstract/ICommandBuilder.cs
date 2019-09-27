using static CodeGen.Context.CodeGenTypelessContext;

namespace CodeGen.Context
{

    public partial class CodeGenContext<TProject, TRootNode, TProcessEntity> 
    {
        public interface ICommandBuilder<TCommand, TNode> : ICommandBuilder<TCommand>
            where TCommand : ICommand<TNode>
        {
            new ITarget<TNode> Target { get; set;}
        }
    }
}
