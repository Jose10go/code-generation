using System;
using CodeGen.Attributes;
using CodeGen.Context;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.Editing;

namespace CodeGen.CSharp.Context
{
    public abstract partial class CSharpContext<TProcessEntity> : CodeGenContext<Solution, CSharpSyntaxNode, TProcessEntity>
    {
        [CommandBuilderModifier]
        public interface IWithNewName<TCommandBuilder,TNode>
            where TCommandBuilder:Core.ICommandBuilder
            where TNode:CSharpSyntaxNode                    
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
