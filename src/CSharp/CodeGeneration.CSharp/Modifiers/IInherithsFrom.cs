using CodeGen.Attributes;
using CodeGen.Context;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using static CodeGen.CSharp.Extensions;
namespace CodeGen.CSharp.Context
{
    public abstract partial class CSharpContext : CodeGenContext<Project, CSharpSyntaxNode,CompilationUnitSyntax, ISymbol>
    {
        [CommandModifier]
        public interface IInheritsFrom<TCommand>
            where TCommand:Core.ICommand
        {
            BaseTypeSyntax InheritsType { get; set; }
            TCommand InheritsFrom(string inheritsType)
            {
                this.InheritsType = SyntaxFactory.SimpleBaseType(SyntaxFactory.ParseTypeName(inheritsType));
                return (TCommand)this;
            }

            TCommand InheritsFrom<T>()
                where T:class
            {
                return this.InheritsFrom(GetCSharpName<T>());
            }

        }
    }
}
