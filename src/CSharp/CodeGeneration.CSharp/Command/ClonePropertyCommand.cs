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
        public interface ICloneProperty : ICommandResult<PropertyDeclarationSyntax>,
                                          ICommandOn<PropertyDeclarationSyntax>,
                                          IGet<ICloneProperty>,
                                          IWithName<ICloneProperty>,
                                          IWithAttribute<ICloneProperty>,
                                          IWithAccessModifier<ICloneProperty>,
                                          IAbstract<ICloneProperty>,
                                          IStatic<ICloneProperty>,
                                          IWithGetSet<ICloneProperty>,
                                          IReturns<ICloneProperty>
        {
        }

        [Command]
        public class ClonePropertyCommand : ICloneProperty
        {
            public string Name { get; set; }
            public SyntaxList<AttributeListSyntax> Attributes{ get; set; }
            public BlockSyntax Body { get; set; }
            public ISingleTarget SingleTarget { get; set ; }
            public SyntaxToken Modifiers { get ; set ; }
            public SyntaxToken Abstract { get ; set ; }
            public SyntaxToken Static { get ; set ; }
            public SyntaxToken Partial { get ; set ; }
            public SyntaxTokenList SetModifier { get ; set ; }
            public SyntaxTokenList GetModifier { get ; set ; }
            public BlockSyntax GetStatements { get ; set ; }
            public BlockSyntax SetStatements { get ; set ; }
            public TypeSyntax ReturnType { get; set ; }
        }
        
    }
}
 