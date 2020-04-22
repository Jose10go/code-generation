﻿using CodeGen.Context;
using CodeGen.Core.Attributes;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CodeGen.CSharp.Context
{
    public abstract partial class CSharpContext : CodeGenContext<Project, CSharpSyntaxNode, CompilationUnitSyntax,ISymbol>
    {
        public interface ICloneInterface : ICommand<InterfaceDeclarationSyntax,InterfaceDeclarationSyntax>,
                                       IGet<ICloneInterface,InterfaceDeclarationSyntax>,
                                       IWithName<ICloneInterface>,
                                       IWithAttribute<ICloneInterface>,
                                       IWithAccessModifier<ICloneInterface>,
                                       IPartial<ICloneInterface>,
                                       IImplements<ICloneInterface>
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
            public ISingleTarget<InterfaceDeclarationSyntax> SingleTarget { get; set; }
            public SyntaxToken Modifiers { get; set; }
            public SyntaxToken Partial { get; set; }
            public string[] ImplementedInterfaces { get ; set ; }
        }

    }
}