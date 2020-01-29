using CodeGen.Attributes;
using CodeGen.Context;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System;

namespace CodeGen.CSharp.Context
{
    public abstract partial class CSharpContext<TProcessEntity> : CodeGenContext<Solution, CSharpSyntaxNode,ISymbol, TProcessEntity>
    {
        public interface IClassClone : ICommandBuilder<ClassDeclarationSyntax>,
                                       IWithNewName<IClassClone, ClassDeclarationSyntax>,
                                       IWithAttribute<IClassClone, ClassDeclarationSyntax>
        {
        }

        [CommandBuilder]
        public class ClassCloneCommandBuilder : IClassClone
        {
            public Func<ClassDeclarationSyntax, string> NewName { get; set; }
            public SyntaxList<AttributeListSyntax> Attributes{ get; set; }
            public ITarget<ClassDeclarationSyntax> Target { get; set; }
        }

    }
}
