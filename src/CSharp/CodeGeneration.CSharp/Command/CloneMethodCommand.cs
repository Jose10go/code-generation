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
        public interface ICloneMethod : ICommandResult<MethodDeclarationSyntax>,
                                        ICommandOn<MethodDeclarationSyntax>,
                                        IGet<ICloneMethod>,
                                        IWithName<ICloneMethod>,
                                        IWithAttribute<ICloneMethod>,
                                        IWithBody<ICloneMethod>,
                                        IWithAccessModifier<ICloneMethod>,
                                        IAbstract<ICloneMethod>,
                                        IStatic<ICloneMethod>,
                                        IPartial<ICloneMethod>,
                                        IReturns<ICloneMethod>,
                                        IWithGeneric<ICloneMethod>,
                                        IWithParameters<ICloneMethod>
        {
        }

        [Command]
        public class CloneMethodCommand : ICloneMethod
        {
            public string Name { get; set; }
            public SyntaxList<AttributeListSyntax> Attributes{ get; set; }
            public BlockSyntax BlockBody { get; set; }
            public ArrowExpressionClauseSyntax ExpressionBody { get ; set ; }
            public ISingleTarget SingleTarget { get; set ; }
            public SyntaxToken Modifiers { get ; set ; }
            public SyntaxToken Abstract { get ; set ; }
            public SyntaxToken Static { get ; set ; }
            public SyntaxToken Partial { get ; set ; }
            public TypeSyntax ReturnType { get ; set ; }
            public TypeParameterListSyntax GenericParameters { get ; set ; }
            public SyntaxList<TypeParameterConstraintClauseSyntax> GenericParametersConstraints { get ; set ; }
            public ParameterListSyntax Parameters { get ; set ; }
        }
        
    }
}
 