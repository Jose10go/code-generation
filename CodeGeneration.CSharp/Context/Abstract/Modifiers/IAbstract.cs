using CodeGen.Attributes;
using CodeGen.Context;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.Collections.Generic;
using System.Linq;
namespace CodeGen.CSharp.Context
{
    public abstract partial class CSharpContext<TProcessEntity> : CodeGenContext<Solution, CSharpSyntaxNode, ISymbol, TProcessEntity>
    {
        [CommandBuilderModifier]
        public interface IAbstract<TCommandBuilder,TNode>
            where TCommandBuilder:Core.ICommandBuilder
            where TNode:CSharpSyntaxNode                    
        {
            SyntaxToken Abstract { get; set; }
            TCommandBuilder MakeAbstract() 
            {
                SyntaxFactory.Token(SyntaxKind.AbstractKeyword);
                return (TCommandBuilder)this;
            }
        }
    }
}
