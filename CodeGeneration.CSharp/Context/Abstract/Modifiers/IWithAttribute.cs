using CodeGen.Attributes;
using CodeGen.Context;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.Collections.Generic;
using System.Linq;
namespace CodeGen.CSharp.Context
{
    public abstract partial class CSharpContext<TProcessEntity> : CodeGenContext<Project, CSharpSyntaxNode,CompilationUnitSyntax,ISymbol, TProcessEntity>
    {
        [CommandModifier]
        public interface IWithAttribute<TCommandBuilder,TNode>
            where TCommandBuilder:Core.ICommand
            where TNode:CSharpSyntaxNode                    
        {
            SyntaxList<AttributeListSyntax> Attributes{ get; set; }
            TCommandBuilder WithAttributes(IEnumerable<string> Attributes) 
            {
                var attrs = Attributes.Select(x => SyntaxFactory.Attribute(SyntaxFactory.ParseName(x)));
                this.Attributes = new SyntaxList<AttributeListSyntax>(SyntaxFactory.AttributeList().AddAttributes(attrs.ToArray()));
                return (TCommandBuilder)this;
            }
        }
    }
}
