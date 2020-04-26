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
        public interface ICreateNamespace : ICommandResult<NamespaceDeclarationSyntax>,
                                            ICommandOn<CompilationUnitSyntax>,
                                            IWithName<ICreateNamespace>,
                                            IUsingNamespace<ICreateNamespace>
        {
        }

        [Command]
        public class CreateNamespaceCommand : ICreateNamespace
        {
            public CreateNamespaceCommand():base()
            {
            }

            public string Name { get; set; }
            public ISingleTarget SingleTarget { get; set; }
            public List<string> Namespaces { get; set; }
        }

    }
}
