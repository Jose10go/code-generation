using CodeGen.Context;
using CodeGen.Core.Attributes;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CodeGen.CSharp.Context
{
    public abstract partial class CSharpContext : CodeGenContext<Project, CSharpSyntaxNode, CompilationUnitSyntax,ISymbol>
    {
        public interface ICreateProperty : ICommand<ClassDeclarationSyntax,PropertyDeclarationSyntax>,
                                         IWithName<ICreateProperty>,
                                         IWithAttribute<ICreateProperty>,
                                         IWithAccessModifier<ICreateProperty>,
                                         IAbstract<ICreateProperty>,
                                         IStatic<ICreateProperty>
        {
        }

        [Command]
        public class CreatePropertyCommand : ICreateProperty
        {
            public CreatePropertyCommand():base()
            {
            }

            public string Name { get; set; }
            public SyntaxList<AttributeListSyntax> Attributes{ get; set; }
            public ISingleTarget<ClassDeclarationSyntax> SingleTarget { get; set; }
            public SyntaxToken Modifiers { get; set; }
            public SyntaxToken Abstract { get; set; }
            public SyntaxToken Static { get; set; }
            public BlockSyntax Body { get ; set ; }
        }

    }
}
