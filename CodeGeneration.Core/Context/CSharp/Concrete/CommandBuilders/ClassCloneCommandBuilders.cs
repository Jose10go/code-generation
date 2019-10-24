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
        public interface IClassCloneCommandBuilder : ICommandBuilder<ClassCloneCommand>,
                                                     IWithNewName<IClassCloneCommandBuilder, ClassCloneCommand, ClassDeclarationSyntax>,
                                                     IWithAttribute<IClassCloneCommandBuilder, ClassCloneCommand, ClassDeclarationSyntax>
        {
        }

        [CommandBuilder]
        public class ClassCloneCommandBuilder : IClassCloneCommandBuilder
        {
            public Func<ClassDeclarationSyntax, string> NewName { get; set; }
            public ICollection<string> Attributes { get; set; }
            public ITarget Target { get ; set ; }
        }
        
        public class ClassCloneCommand : ICommand<ClassDeclarationSyntax>
        {
            public Func<ClassDeclarationSyntax, string> NewName { get; set; }
            public ICollection<string> Attributes { get; set; }
            public ITarget Target { get; set; }
        }

    }
}
 