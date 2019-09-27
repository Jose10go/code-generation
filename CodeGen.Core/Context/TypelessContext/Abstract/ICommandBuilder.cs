namespace CodeGen.Context
{
    public partial class CodeGenTypelessContext
    {
        public interface ICommandBuilder<TCommand> where TCommand : ICommand
        {
            TCommand Build();
            ITarget Target { get; set; }
        }
    }
}
