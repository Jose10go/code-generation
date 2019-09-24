using System;
using static CodeGen.Context.CodeGenTypelessContext;

namespace CodeGen.Context
{

    public partial class CodeGenContext<TProject, TRootNode, TProcessEntity> 
    {
        public interface ITargetBuilder<TNode> : ITargetBuilder
        {
            ITargetBuilder<TNode> Where(Func<TNode, bool> filter);
            new ITarget<TRootNode> Build();
            new ICommandBuilder<TCommand> Execute<TCommand>() where TCommand : ICommand<TNode>;
        }
    }
}
