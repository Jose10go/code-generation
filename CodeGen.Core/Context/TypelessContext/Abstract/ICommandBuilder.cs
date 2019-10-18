namespace CodeGen.Context
{
    public partial class CodeGenTypelessContext
    {
        public interface ICommandBuilder<out TCommand> where TCommand : ICommand
        {
            TCommand Build();
            ITarget Target { get; set; }
            void Go(object processEntity);
        }
    }
}
