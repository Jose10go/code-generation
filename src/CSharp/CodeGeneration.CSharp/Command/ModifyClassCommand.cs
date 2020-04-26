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
        public interface IModifyClass :ICommandResult<ClassDeclarationSyntax>,
                                       ICommandOn<ClassDeclarationSyntax>,
                                       IGet<IModifyClass>,
                                       IWithName<IModifyClass>,
                                       IWithAttribute<IModifyClass>,
                                       IWithAccessModifier<IModifyClass>,
                                       IAbstract<IModifyClass>,
                                       IStatic<IModifyClass>,
                                       IPartial<IModifyClass>,
                                       IInheritsFrom<IModifyClass>,
                                       IImplements<IModifyClass>,
                                       IWithGeneric<IModifyClass>
        {
        }

        [Command]
        public class ModifyClassCommand : IModifyClass
        {
            public ModifyClassCommand():base()
            {
            }

            public string Name { get; set; }
            public SyntaxList<AttributeListSyntax> Attributes{ get; set; }
            public ISingleTarget SingleTarget { get; set; }
            public SyntaxToken Modifiers { get; set; }
            public SyntaxToken Abstract { get; set; }
            public SyntaxToken Static { get; set; }
            public SyntaxToken Partial { get; set; }
            public BaseTypeSyntax InheritsType { get ; set ; }
            public BaseListSyntax ImplementedInterfaces { get; set; }
            public TypeParameterListSyntax GenericParameters { get ; set ; }
            public SyntaxList<TypeParameterConstraintClauseSyntax> GenericParametersConstraints { get ; set ; }
        }

    }
}
