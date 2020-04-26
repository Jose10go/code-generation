﻿using CodeGen.Context;
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
        public interface ICreateClass: ICommandResult<ClassDeclarationSyntax>,
                                       ICommandOn<NamespaceDeclarationSyntax>,
                                       ICommandOn<ClassDeclarationSyntax>,
                                       ICommandOn<StructDeclarationSyntax>,
                                       ICommandOn<InterfaceDeclarationSyntax>,
                                       IGet<ICreateClass>,
                                       IWithName<ICreateClass>,
                                       IWithAttribute<ICreateClass>,
                                       IWithAccessModifier<ICreateClass>,
                                       IAbstract<ICreateClass>,
                                       IStatic<ICreateClass>,
                                       IPartial<ICreateClass>,
                                       IInheritsFrom<ICreateClass>,
                                       IImplements<ICreateClass>,
                                       IWithGeneric<ICreateClass>
        {
        }

        [Command]
        public class CreateClassCommand : ICreateClass
        {
            public CreateClassCommand():base()
            {
            }

            public string Name { get; set; }
            public SyntaxList<AttributeListSyntax> Attributes{ get; set; }
            public ISingleTarget SingleTarget { get; set; }
            public SyntaxToken Modifiers { get; set; }
            public SyntaxToken Abstract { get; set; }
            public SyntaxToken Static { get; set; }
            public SyntaxToken Partial { get; set; }
            public string InheritsType { get ; set ; }
            public string[] ImplementedInterfaces { get ; set ; }
            public Dictionary<string, List<string>> GenericTypes { get; set; }
        }

    }
}
