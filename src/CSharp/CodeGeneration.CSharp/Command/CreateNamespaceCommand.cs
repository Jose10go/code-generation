using CodeGen.Context;
using CodeGen.Core.Attributes;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CodeGen.CSharp.Context
{
    public abstract partial class CSharpContext<TProcessEntity> : CodeGenContext<Project, CSharpSyntaxNode, CompilationUnitSyntax,ISymbol, TProcessEntity>
    {
        public interface ICreateNamespace : ICommand<CompilationUnitSyntax,NamespaceDeclarationSyntax>,
                                            IWithName<ICreateNamespace>
        {
        }

        [Command]
        public class CreateNamespaceCommand : ICreateNamespace
        {
            public CreateNamespaceCommand():base()
            {
            }

            public string Name { get; set; }
            public ISingleTarget<CompilationUnitSyntax> SingleTarget { get; set; }
        }

    }
}
