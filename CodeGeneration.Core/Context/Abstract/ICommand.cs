
namespace CodeGen.Context
{
    public partial class CodeGenContext<TProject, TBaseNode, TRootNode, TSemanticModel, TProcessEntity>
        where TRootNode:TBaseNode
    {
        public interface ICommand<TSyntaxNode> : Core.ICommand,ITarget<TSyntaxNode>
            where TSyntaxNode:TBaseNode
        {
            Target<TSyntaxNode> Target { get; set; }
        }
    }
}