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
        public interface ICloneInterface : ICommandResult<InterfaceDeclarationSyntax>,
                                           ICommandOn<InterfaceDeclarationSyntax>,
                                           IGet<ICloneInterface>,
                                           IWithName<ICloneInterface>,
                                           IWithAttribute<ICloneInterface>,
                                           IWithAccessModifier<ICloneInterface>,
                                           IPartial<ICloneInterface>,
                                           IImplements<ICloneInterface>,
                                           IWithGeneric<ICloneInterface>
        {
        }

        [Command]
        public class CloneInterfaceCommand : ICloneInterface
        {
            public CloneInterfaceCommand():base()
            {
            }

            public string Name { get; set; }
            public SyntaxList<AttributeListSyntax> Attributes{ get; set; }
            public ISingleTarget SingleTarget { get; set; }
            public SyntaxToken Modifiers { get; set; }
            public SyntaxToken Partial { get; set; }
            public BaseListSyntax ImplementedInterfaces { get ; set ; }
            public TypeParameterListSyntax GenericParameters { get ; set ; }
            public SyntaxList<TypeParameterConstraintClauseSyntax> GenericParametersConstraints { get ; set ; }
        }

    }
}
