using CodeGen.Core;

namespace CodeGen.Context
{
    public partial class CodeGenContext<TProject, TBaseNode, TRootNode, TSemanticModel>
        where TRootNode:TBaseNode
    {
        public interface ICommandOn<TExecuteOnNode> : ICommand
            where TExecuteOnNode : TBaseNode
        {
        }

        public interface ICommandResult<TOutNode> : ICommand
            where TOutNode : TBaseNode
        {
        }
    }
}