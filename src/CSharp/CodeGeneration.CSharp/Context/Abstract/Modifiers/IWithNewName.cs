using System;
using CodeGen.Attributes;
using CodeGen.Context;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CodeGen.CSharp.Context
{
    public abstract partial class CSharpContext<TProcessEntity> : CodeGenContext<Project, CSharpSyntaxNode, CompilationUnitSyntax, ISymbol, TProcessEntity>
    {
        [CommandModifier]
        public interface IWithNewName<TCommandBuilder,TNode>
            where TCommandBuilder:Core.ICommand
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
