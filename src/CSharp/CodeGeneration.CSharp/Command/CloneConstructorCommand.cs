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
        public interface ICloneConstructor : ICommandResult<ConstructorDeclarationSyntax>,
                                             ICommandOn<ConstructorDeclarationSyntax>,
                                             IGet<ICloneConstructor>,
                                             IWithAttribute<ICloneConstructor>,
                                             IWithBody<ICloneConstructor>,
                                             IWithAccessModifier<ICloneConstructor>,
                                             IStatic<ICloneConstructor>,
                                             IWithParameters<ICloneConstructor>,
                                             IOn<ICloneConstructor,ClassDeclarationSyntax>,
                                             IOn<ICloneConstructor,StructDeclarationSyntax>
        {
        }

        [Command]
        public class CloneConstructorCommand : ICloneConstructor
        {
            public string Name { get; set; }
            public SyntaxList<AttributeListSyntax> Attributes{ get; set; }
            public BlockSyntax BlockBody { get; set; }
            public ArrowExpressionClauseSyntax ExpressionBody { get ; set ; }
            public ISingleTarget SingleTarget { get; set ; }
            public SyntaxToken Modifiers { get ; set ; }
            public SyntaxToken Static { get ; set ; }
            public ParameterListSyntax Parameters { get ; set ; }
            public CSharpSyntaxNode OnNode { get ; set ; }
        }
    }
}
 