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
        public interface ICloneClass : ICommandResult<ClassDeclarationSyntax>,
                                       ICommandOn<ClassDeclarationSyntax>,
                                       IGet<ICloneClass>,
                                       IWithName<ICloneClass>,
                                       IWithAttribute<ICloneClass>,
                                       IWithAccessModifier<ICloneClass>,
                                       IAbstract<ICloneClass>,
                                       IStatic<ICloneClass>,
                                       IPartial<ICloneClass>,
                                       IInheritsFrom<ICloneClass>,
                                       IImplements<ICloneClass>,
                                       IWithGeneric<ICloneClass>
        {
        }

        [Command]
        public class CloneClassCommand : ICloneClass
        {
            public CloneClassCommand():base()
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
            public Dictionary<string, List<string>> GenericTypes { get; set ; }
        }

    }
}
