using CodeGen.Context;
using CodeGen.Core;
using CodeGen.Core.Attributes;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CodeGen.CSharp.Context
{
    public abstract partial class CSharpContext : CodeGenContext<Project, CSharpSyntaxNode, CompilationUnitSyntax,ISymbol>
    {
        public interface ICreateProperty:ICommandResult<PropertyDeclarationSyntax>,
                                         ICommandOn<ClassDeclarationSyntax>,
                                         ICommandOn<StructDeclarationSyntax>,
                                         ICommandOn<InterfaceDeclarationSyntax>,
                                         IWithName<ICreateProperty>,
                                         IGet<ICreateProperty>,
                                         IWithAttribute<ICreateProperty>,
                                         IWithAccessModifier<ICreateProperty>,
                                         IAbstract<ICreateProperty>,
                                         IStatic<ICreateProperty>,
                                         IWithGetSet<ICreateProperty>,
                                         IReturns<ICreateProperty>
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
            public ISingleTarget SingleTarget { get; set; }
            public SyntaxToken Modifiers { get; set; }
            public SyntaxToken Abstract { get; set; }
            public SyntaxToken Static { get; set; }
            public SyntaxTokenList SetModifier { get ; set ; }
            public SyntaxTokenList GetModifier { get ; set ; }
            public BlockSyntax GetStatements { get ; set ; }
            public BlockSyntax SetStatements { get ; set ; }
            public TypeSyntax ReturnType { get; set ; }
        }

    }
}
