using static CodeGen.Context.CodeGenTypelessContext;

namespace CodeGen.Context
{

    public partial class CodeGenContext<TProject, TRootNode, TProcessEntity> 
    {
        public interface ICommandHandler<TCommand> : ICommandHandler
            where TCommand : ICommand
        {
            TCommand Command { get; set; }

            TProcessEntity ProcessDocument(TProcessEntity entity);
        }
    }
}
