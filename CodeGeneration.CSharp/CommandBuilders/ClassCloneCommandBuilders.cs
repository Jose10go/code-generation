using CodeGen.Attributes;
using CodeGen.Context;
using CodeGen.Core;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System;
using System.Collections.Generic;

namespace CodeGen.CSharp.Context
{
    public abstract partial class CSharpContext<TProcessEntity> : CodeGenContext<Solution, CSharpSyntaxNode, TProcessEntity>
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
            public ICollection<string> Attributes { get; set; }
            public ITarget<ClassDeclarationSyntax> Target { get; set; }
        }

    }
}
