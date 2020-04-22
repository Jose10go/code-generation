using CodeGen.Context;
using CodeGen.Core.Attributes;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CodeGen.CSharp.Context
{
    public abstract partial class CSharpContext : CodeGenContext<Project, CSharpSyntaxNode, CompilationUnitSyntax,ISymbol>
    {
        public interface ICreateInterface: ICommand<NamespaceDeclarationSyntax, InterfaceDeclarationSyntax>,
                                       IGet<ICreateInterface,NamespaceDeclarationSyntax>,
                                       IWithName<ICreateInterface>,
                                       IWithAttribute<ICreateInterface>,
                                       IWithAccessModifier<ICreateInterface>,
                                       IPartial<ICreateInterface>,
                                       IImplements<ICreateInterface>
        {
        }

        [Command]
        public class CreateInterfaceCommand : ICreateInterface
        {
            public CreateInterfaceCommand():base()
            {
            }

            public string Name { get; set; }
            public SyntaxList<AttributeListSyntax> Attributes{ get; set; }
            public ISingleTarget<NamespaceDeclarationSyntax> SingleTarget { get; set; }
            public SyntaxToken Modifiers { get; set; }
            public SyntaxToken Partial { get; set; }
            public string[] ImplementedInterfaces { get ; set ; }
        }

    }
}
