using System;

namespace CodeGen.Context
{
    public partial class CodeGenTypelessContext
    {
        public interface ITargetBuilder
        {
            ITargetBuilder Where(Func<object, bool> filter);
            ITarget Build();
            TCommandBuilder Execute<TCommand,TCommandBuilder>() 
                where TCommand : ICommand 
                where TCommandBuilder:ICommandBuilder<TCommand>;
        }
    }
}
