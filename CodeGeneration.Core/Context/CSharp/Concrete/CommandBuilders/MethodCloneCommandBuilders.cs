using CodeGen.Attributes;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System;
using System.Collections.Generic;
using static CodeGen.Context.CodeGenTypelessContext;

namespace CodeGen.Context.CSharp
{
    public partial class CSharpContext<TProcessEntity> : CodeGenContext<Solution, CSharpSyntaxNode, TProcessEntity>
    {
        public interface IMethodCloneCommandBuilder : ICommandBuilder<MethodCloneCommand>,
                                                      IWithNewName<IMethodCloneCommandBuilder, MethodCloneCommand, MethodDeclarationSyntax>,
                                                      IWithAttribute<IMethodCloneCommandBuilder, MethodCloneCommand, MethodDeclarationSyntax>
        {
        }

        [CommandBuilder]
        public class MethodCloneCommandBuilder : IMethodCloneCommandBuilder
        {
            public Func<MethodDeclarationSyntax, string> NewName { get; set; }
            public ICollection<string> Attributes { get; set; }
            public ITarget Target { get ; set ; }
        }
        
        public class MethodCloneCommand : ICommand<MethodDeclarationSyntax>
        {
            public Func<MethodDeclarationSyntax, string> NewName { get; set; }
            public ICollection<string> Attributes { get; set; }
            public ITarget Target { get; set; }
        }

    }
}
 