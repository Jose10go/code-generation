using System;
using CodeGen.Attributes;
using CodeGen.Context;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.Collections.Generic;
using System.Collections;

namespace CodeGen.CSharp.Context
{
    public abstract partial class CSharpContext<TProcessEntity> : CodeGenContext<Project, CSharpSyntaxNode, ISymbol, TProcessEntity>
    {
        [CommandBuilderModifier]
        public interface IWithNewArgument<TCommandBuilder,TNode>
            where TCommandBuilder:Core.ICommandBuilder
            where TNode:CSharpSyntaxNode                    
        {
            IList<(Func<TNode,ArgumentSyntax>, int)> NewArguments { get; set; }
            TCommandBuilder WithNewArgument(int position,Func<TNode,ArgumentSyntax> NewArgument) 
            {
                this.NewArguments.Add((NewArgument,position));
                return (TCommandBuilder)this;
            } 
        }
    }
}
