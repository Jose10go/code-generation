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
        public interface ICreateConstructor : ICommandResult<ConstructorDeclarationSyntax>,
                                              ICommandOn<ClassDeclarationSyntax>,
                                              ICommandOn<StructDeclarationSyntax>,
                                              IGet<ICreateConstructor>,
                                              IWithAttribute<ICreateConstructor>,
                                              IWithBody<ICreateConstructor>,
                                              IWithAccessModifier<ICreateConstructor>,
                                              IStatic<ICreateConstructor>,
                                              IWithParameters<ICreateConstructor>
        {
        }

        [Command]
        public class CreateConstructorCommand : ICreateConstructor
        {
            public CreateConstructorCommand():base()
            {
            }

            public string Name { get; set; }
            public SyntaxList<AttributeListSyntax> Attributes{ get; set; }
            public ISingleTarget SingleTarget { get; set; }
            public SyntaxToken Modifiers { get; set; }
            public SyntaxToken Static { get; set; }
            public BlockSyntax BlockBody { get ; set ; }
            public ArrowExpressionClauseSyntax ExpressionBody { get ; set ; }
            public ParameterListSyntax Parameters { get; set ; }
        }

    }
}
