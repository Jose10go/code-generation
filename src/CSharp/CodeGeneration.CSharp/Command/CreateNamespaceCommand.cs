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
        public class CreateNamespaceCommand : CSharpTarget<NamespaceDeclarationSyntax>, ICreateNamespace
        {
            public CreateNamespaceCommand(ICodeGenerationEngine engine):base(engine)
            {
            }

            public string Name { get; set; }
            public ITarget<CompilationUnitSyntax> Target { get; set; }
        }

    }
}
