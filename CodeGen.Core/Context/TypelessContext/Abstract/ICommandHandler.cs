namespace CodeGen.Context
{
    public partial class CodeGenTypelessContext
    {
        public interface ICommandHandler
        {
            ITarget Target { get; }

            ICommand Command { get; }

            object ProcessDocument(object entity);
        }
    }
}
