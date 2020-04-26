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
        public interface IModifyStruct: ICommandResult<StructDeclarationSyntax>,
                                        ICommandOn<StructDeclarationSyntax>,
                                        IGet<IModifyStruct>,
                                        IWithName<IModifyStruct>,
                                        IWithAttribute<IModifyStruct>,
                                        IWithAccessModifier<IModifyStruct>,
                                        IPartial<IModifyStruct>,
                                        IImplements<IModifyStruct>,
                                        IWithGeneric<IModifyStruct>
        {
        }

        [Command]
        public class ModifyStructCommand : IModifyStruct
        {
            public ModifyStructCommand():base()
            {
            }

            public string Name { get; set; }
            public SyntaxList<AttributeListSyntax> Attributes{ get; set; }
            public ISingleTarget SingleTarget { get; set; }
            public SyntaxToken Modifiers { get; set; }
            public SyntaxToken Partial { get; set; }
            public BaseListSyntax ImplementedInterfaces { get; set; }
            public TypeParameterListSyntax GenericParameters { get; set ; }
            public SyntaxList<TypeParameterConstraintClauseSyntax> GenericParametersConstraints { get ; set ; }
        }

    }
}
