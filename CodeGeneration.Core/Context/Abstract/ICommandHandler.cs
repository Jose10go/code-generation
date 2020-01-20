using CodeGen.Core;

namespace CodeGen.Context
{
    public abstract partial class CodeGenContext<TProject, TRootNode, TProcessEntity> 
    {
        public interface ICommandHandler: Core.ICommandHandler
        {
            void ProcessDocument(TProcessEntity processEntity);
        }
    }
}
