
namespace CodeGen.Context
{
    public partial class CodeGenContext<TProject, TBaseNode, TRootNode, TSemanticModel, TProcessEntity>
        where TRootNode:TBaseNode
    {
        public interface ICommand<TExecuteOnNode> : Core.ICommand
            where TExecuteOnNode : TBaseNode
        {
            ITarget<TExecuteOnNode> Target { get; set; }
        }

        public interface ICommand<TExecuteOnNode, TAllowExecuteOnNode> : ICommand<TExecuteOnNode>, ITarget<TAllowExecuteOnNode>
            where TExecuteOnNode : TBaseNode
            where TAllowExecuteOnNode : TBaseNode
        {
        }
    }
}