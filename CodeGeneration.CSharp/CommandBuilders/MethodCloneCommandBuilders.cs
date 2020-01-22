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
        public interface IMethodClone : ICSharpCommandBuilder,
                                        IWithNewName<IMethodClone,MethodDeclarationSyntax>,
                                        IWithAttribute<IMethodClone, MethodDeclarationSyntax>
        {
        }

        [CommandBuilder]
        public class MethodCloneCommandBuilder : IMethodClone
        {
            public Func<MethodDeclarationSyntax, string> NewName { get; set; }
            public ICollection<string> Attributes { get; set; }
            public ITarget Target { get ; set ; }

        }
        
    }
}
 