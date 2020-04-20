namespace CodeGen.Context
{
    public partial class CodeGenContext<TProject, TBaseNode, TRootNode, TSemanticModel, TProcessEntity>
        where TRootNode:TBaseNode
    {
        public interface ICommand<TExecuteOnNode> : Core.ICommand
            where TExecuteOnNode : TBaseNode
        {
             ISingleTarget<TExecuteOnNode> SingleTarget { get; set; }
        }

        public interface ICommand<TExecuteOnNode, TOutNode> : ICommand<TExecuteOnNode>
            where TExecuteOnNode : TBaseNode
            where TOutNode : TBaseNode
        {
        }
    }
}