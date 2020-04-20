using CodeGen.Attributes;
using CodeGen.Context;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.Collections.Generic;
using System.Linq;
namespace CodeGen.CSharp.Context
{
    public abstract partial class CSharpContext : CodeGenContext<Project, CSharpSyntaxNode,CompilationUnitSyntax,ISymbol>
    {
        [CommandModifier]
        public interface IWithAttribute<TCommandBuilder>
            where TCommandBuilder:Core.ICommand
        {
            SyntaxList<AttributeListSyntax> Attributes{ get; set; }
            TCommandBuilder WithAttributes(IEnumerable<string> Attributes) 
            {
                var attrs = Attributes.Select(x => SyntaxFactory.Attribute(SyntaxFactory.ParseName(x)));
                this.Attributes = new SyntaxList<AttributeListSyntax>(SyntaxFactory.AttributeList().AddAttributes(attrs.ToArray()));
                return (TCommandBuilder)this;
            }

            TCommandBuilder WithAttributes(params string[] Attributes)
            {
                return this.WithAttributes(Attributes.AsEnumerable());
            }
        }
    }
}
