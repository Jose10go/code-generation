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
        public interface ICreateMethod : ICommandResult<MethodDeclarationSyntax>,
                                         ICommandOn<ClassDeclarationSyntax>,
                                         ICommandOn<StructDeclarationSyntax>,
                                         ICommandOn<InterfaceDeclarationSyntax>,
                                         ICommandOn<MethodDeclarationSyntax>,
                                         IWithName<ICreateMethod>,
                                         IGet<ICreateMethod>,
                                         IWithAttribute<ICreateMethod>,
                                         IWithBody<ICreateMethod>,
                                         IWithAccessModifier<ICreateMethod>,
                                         IAbstract<ICreateMethod>,
                                         IStatic<ICreateMethod>,
                                         IPartial<ICreateMethod>,
                                         IReturns<ICreateMethod>,
                                         IWithGeneric<ICreateMethod>,
                                         IWithParameters<ICreateMethod>
        {
        }

        [Command]
        public class CreateMethodCommand : ICreateMethod
        {
            public CreateMethodCommand():base()
            {
            }

            public string Name { get; set; }
            public TypeSyntax ReturnType { get; set; }
            public SyntaxList<AttributeListSyntax> Attributes{ get; set; }
            public ISingleTarget SingleTarget { get; set; }
            public SyntaxToken Modifiers { get; set; }
            public SyntaxToken Abstract { get; set; }
            public SyntaxToken Static { get; set; }
            public SyntaxToken Partial { get; set; }
            public BlockSyntax BlockBody { get ; set ; }
            public ArrowExpressionClauseSyntax ExpressionBody { get ; set ; }
            public TypeParameterListSyntax GenericParameters { get ; set ; }
            public SyntaxList<TypeParameterConstraintClauseSyntax> GenericParametersConstraints { get ; set ; }
            public ParameterListSyntax Parameters { get; set ; }
        }

    }
}
