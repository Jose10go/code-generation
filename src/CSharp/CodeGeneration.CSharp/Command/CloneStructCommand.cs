﻿using CodeGen.Context;
using CodeGen.Core.Attributes;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.Collections.Generic;

namespace CodeGen.CSharp.Context
{
    public abstract partial class CSharpContext : CodeGenContext<Project, CSharpSyntaxNode, CompilationUnitSyntax,ISymbol>
    {
        public interface ICloneStruct: ICommand<StructDeclarationSyntax, StructDeclarationSyntax>,
                                       IGet<ICloneStruct, StructDeclarationSyntax>,
                                       IWithName<ICloneStruct>,
                                       IWithAttribute<ICloneStruct>,
                                       IWithAccessModifier<ICloneStruct>,
                                       IPartial<ICloneStruct>,
                                       IImplements<ICloneStruct>,
                                       IWithGeneric<ICloneStruct>
        {
        }

        [Command]
        public class CloneStructCommand : ICloneStruct
        {
            public CloneStructCommand():base()
            {
            }

            public string Name { get; set; }
            public SyntaxList<AttributeListSyntax> Attributes{ get; set; }
            public ISingleTarget<StructDeclarationSyntax> SingleTarget { get; set; }
            public SyntaxToken Modifiers { get; set; }
            public SyntaxToken Partial { get; set; }
            public string[] ImplementedInterfaces { get ; set ; }
            public Dictionary<string, List<string>> GenericTypes { get ; set ; }
        }

    }
}
