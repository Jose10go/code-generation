
namespace CodeGen.Context
{
    public partial class CodeGenContext<TProject, TBaseNode, TRootNode, TSemanticModel, TProcessEntity>
        where TRootNode:TBaseNode
    {
        public interface ICommand<TExecuteOnNode, TOutNode> : Core.ICommand
            where TExecuteOnNode : TBaseNode
            where TOutNode : TBaseNode
        {
            //This method is used only to force TypeInference on Execute...
            TOutNode Go() => default;
        }
    }
}