using CodeGen.Attributes;
using System.Collections.Generic;
using static CodeGen.Context.CodeGenTypelessContext;

namespace CodeGen.Context
{
    public partial class CodeGenContext<TProject, TRootNode, TProcessEntity>
    {
        [CommandBuilderModifier]
        public interface IWithAttribute<TCommandBuilder,TCommand,TNode>
            where TCommandBuilder:ICommandBuilder<TCommand>
            where TCommand:ICommand,new()
            where TNode:TRootNode                    
        {
            ICollection<string> Attributes{ get; set; }
            TCommandBuilder WithAttributes(ICollection<string> Attributes) 
            {
                this.Attributes =Attributes;
                return (TCommandBuilder)this;
            } 
        }
    }
}
