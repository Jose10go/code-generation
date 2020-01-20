using CodeGen.Attributes;
using CodeGen.Core;
using System.Collections.Generic;

namespace CodeGen.Context
{
    public abstract partial class CodeGenContext<TProject, TRootNode, TProcessEntity>
    {
        [CommandBuilderModifier]
        public interface IWithAttribute<TCommandBuilder,TNode>
            where TCommandBuilder:Core.ICommandBuilder
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
