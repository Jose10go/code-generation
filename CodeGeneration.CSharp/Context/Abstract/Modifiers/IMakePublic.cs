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
        public interface IMakePublic<TCommandBuilder,TNode>
            where TCommandBuilder:Core.ICommandBuilder
            where TNode:CSharpSyntaxNode                    
        {
            SyntaxTokenList Modifiers { get; set; }
            TCommandBuilder MakePublic() 
            {
                Modifiers.Add(SyntaxFactory.Token(SyntaxKind.PublicKeyword));
                return (TCommandBuilder)this;
            }
        }
    }
}
