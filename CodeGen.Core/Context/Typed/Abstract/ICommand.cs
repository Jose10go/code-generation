using static CodeGen.Context.CodeGenTypelessContext;

namespace CodeGen.Context
{

    public partial class CodeGenContext<TProject, TRootNode, TProcessEntity> 
    {
        public interface ICommand<out TSyntaxNode> : ICommand
        {
            new ITarget<TSyntaxNode> Target { get;}
        }
    }
}
