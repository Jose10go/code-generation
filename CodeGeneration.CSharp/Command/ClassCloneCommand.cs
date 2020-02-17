using CodeGen.Attributes;
using CodeGen.Context;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System;

namespace CodeGen.CSharp.Context
{
    public abstract partial class CSharpContext<TProcessEntity> : CodeGenContext<Project, CSharpSyntaxNode,ISymbol, TProcessEntity>
    {
        public interface IClassClone : ICommand<ClassDeclarationSyntax>,
                                       IWithNewName<IClassClone, ClassDeclarationSyntax>,
                                       IWithAttribute<IClassClone, ClassDeclarationSyntax>,
                                       IWithAccessModifier<IClassClone,ClassDeclarationSyntax>,
                                       IAbstract<IMethodClone, ClassDeclarationSyntax>,
                                       IStatic<IMethodClone, ClassDeclarationSyntax>,
                                       IPartial<IMethodClone, ClassDeclarationSyntax>
        {
        }

        [CommandBuilder]
        public class ClassCloneCommand : IClassClone
        {
            public Func<ClassDeclarationSyntax, string> NewName { get; set; }
            public SyntaxList<AttributeListSyntax> Attributes{ get; set; }
            public Target<ClassDeclarationSyntax> Target { get; set; }
            public SyntaxToken Modifiers { get; set; }
            public SyntaxToken Abstract { get; set; }
            public SyntaxToken Static { get; set; }
            public SyntaxToken Partial { get; set; }
        }

    }
}
