using System;
using CodeGen.Attributes;
using static CodeGen.Context.CodeGenTypelessContext;

namespace CodeGen.Context
{
    public partial class CodeGenContext<TProject, TRootNode, TProcessEntity>
    {
        [CommandBuilderModifier]
        public interface IWithNewName<TCommandBuilder,TCommand,TNode>
            where TCommandBuilder:ICommandBuilder<TCommand>
            where TCommand:ICommand,new()
            where TNode:TRootNode                    
        {
            Func<TNode, string> NewName { get; set; }
            TCommandBuilder WithNewName(Func<TNode, string> NewName) 
            {
                this.NewName = NewName;
                return (TCommandBuilder)this;
            } 
        }
    }
}
