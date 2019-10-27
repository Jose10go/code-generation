using System.Linq;

namespace CodeGen.Context
{
    public partial class CodeGenTypelessContext
    {
        //Markup Interface
        public interface ICommandBuilder
        {
            ITarget Target { get; set; }
        }
    }
}
