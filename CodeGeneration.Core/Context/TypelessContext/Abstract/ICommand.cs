namespace CodeGen.Context
{
    public partial class CodeGenTypelessContext
    {
        public interface ICommand
        {
            ITarget Target { get; set; }
        }
    }
}
