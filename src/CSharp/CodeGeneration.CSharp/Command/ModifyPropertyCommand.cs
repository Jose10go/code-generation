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
        public interface IModifyProperty : ICommandResult<PropertyDeclarationSyntax>,
                                        ICommandOn<PropertyDeclarationSyntax>,
                                        IGet<IModifyProperty>,
                                        IWithName<IModifyProperty>,
                                        IWithAttribute<IModifyProperty>,
                                        IWithAccessModifier<IModifyProperty>,
                                        IAbstract<IModifyProperty>,
                                        IStatic<IModifyProperty>,
                                        IReturns<IModifyProperty>,
                                        IWithGetSet<IModifyProperty>
        {
        }

        [Command]
        public class ModifyPropertyCommand : IModifyProperty
        {
            public string Name { get; set; }
            public SyntaxList<AttributeListSyntax> Attributes{ get; set; }
            public ISingleTarget SingleTarget { get; set ; }
            public SyntaxToken Modifiers { get ; set ; }
            public SyntaxToken Abstract { get ; set ; }
            public SyntaxToken Static { get ; set ; }
            public TypeSyntax ReturnType { get; set ; }
            public SyntaxTokenList GetModifier { get ; set ; }
            public SyntaxTokenList SetModifier { get ; set ; }
            public BlockSyntax GetStatements { get ; set ; }
            public BlockSyntax SetStatements { get ; set ; }
        }
        
    }
}
 