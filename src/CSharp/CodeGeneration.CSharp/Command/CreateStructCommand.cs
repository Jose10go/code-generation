using CodeGen.Context;
using CodeGen.Core;
using CodeGen.Core.Attributes;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.Collections.Generic;

namespace CodeGen.CSharp.Context
{
    public abstract partial class CSharpContext : CodeGenContext<Project, CSharpSyntaxNode, CompilationUnitSyntax,ISymbol>
    {
        public interface ICreateStruct: ICommandResult<StructDeclarationSyntax>, 
                                        ICommandOn<NamespaceDeclarationSyntax>,
                                        ICommandOn<ClassDeclarationSyntax>,
                                        ICommandOn<StructDeclarationSyntax>,
                                        ICommandOn<InterfaceDeclarationSyntax>,
                                        IGet<ICreateStruct>,
                                        IWithName<ICreateStruct>,
                                        IWithAttribute<ICreateStruct>,
                                        IWithAccessModifier<ICreateStruct>,
                                        IPartial<ICreateStruct>,
                                        IImplements<ICreateStruct>,
                                        IWithGeneric<ICreateStruct>
        {
        }

        [Command]
        public class CreateStructCommand : ICreateStruct
        {
            public CreateStructCommand():base()
            {
            }

            public string Name { get; set; }
            public SyntaxList<AttributeListSyntax> Attributes{ get; set; }
            public ISingleTarget SingleTarget { get; set; }
            public SyntaxToken Modifiers { get; set; }
            public SyntaxToken Partial { get; set; }
            public BaseListSyntax ImplementedInterfaces { get ; set ; }
            public TypeParameterListSyntax GenericParameters { get ; set; }
            public SyntaxList<TypeParameterConstraintClauseSyntax> GenericParametersConstraints { get ; set ; }
        }

    }
}
