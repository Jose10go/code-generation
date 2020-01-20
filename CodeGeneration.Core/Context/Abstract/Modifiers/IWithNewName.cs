using System;
using CodeGen.Attributes;
using CodeGen.Core;

namespace CodeGen.Context
{
    public abstract partial class CodeGenContext<TProject, TRootNode, TProcessEntity>
    {
        [CommandBuilderModifier]
        public interface IWithNewName<TCommandBuilder,TNode>
            where TCommandBuilder:Core.ICommandBuilder
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
