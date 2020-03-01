using CodeGen.Core;

namespace CodeGen.Context
{
    public abstract partial class CodeGenContext<TProject, TBaseNode,TRootNode,TSemanticModel, TProcessEntity> 
        where TRootNode:TBaseNode
    {
        public interface ICommandHandler : Core.ICommandHandler
        {
            bool ProcessDocument(TProcessEntity processEntity);
        }

        //keyInterface
        public interface ICommandHandler<TCommand> : ICommandHandler
            where TCommand:ICommand
        {
        }
    }
}
