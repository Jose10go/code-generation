using CodeGen.Attributes;
using CodeGen.Context;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CodeGen.CSharp.Context
{
    public abstract partial class CSharpContext : CodeGenContext<Project, CSharpSyntaxNode, CompilationUnitSyntax, ISymbol>
    {
        [CommandModifier]
        public interface IWithName<TCommandBuilder>
            where TCommandBuilder:Core.ICommand
        {
            string Name { get; set; }
            TCommandBuilder WithName(string Name) 
            {
                this.Name = Name;
                return (TCommandBuilder)this;
            } 
        }
    }
}
