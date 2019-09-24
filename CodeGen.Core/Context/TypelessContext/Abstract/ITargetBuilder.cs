using System;

namespace CodeGen.Context
{
    public partial class CodeGenTypelessContext
    {
        public interface ITargetBuilder
        {
            ITargetBuilder Where(Func<object, bool> filter);
            ITarget Build();
            ICommandBuilder<TCommand> Execute<TCommand>() where TCommand : ICommand;
        }
    }
}
