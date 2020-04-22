using CodeGen.Context;
using CodeGen.Core.Attributes;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CodeGen.CSharp.Context
{
    public abstract partial class CSharpContext : CodeGenContext<Project, CSharpSyntaxNode, CompilationUnitSyntax,ISymbol>
    {
        public interface IModifyInterface : ICommand<InterfaceDeclarationSyntax,InterfaceDeclarationSyntax>,
                                       IGet<IModifyInterface,InterfaceDeclarationSyntax>,
                                       IWithName<IModifyInterface>,
                                       IWithAttribute<IModifyInterface>,
                                       IWithAccessModifier<IModifyInterface>,
                                       IPartial<IModifyInterface>,
                                       IImplements<IModifyInterface>
        {
        }

        [Command]
        public class ModifyInterfaceCommand : IModifyInterface
        {
            public ModifyInterfaceCommand():base()
            {
            }

            public string Name { get; set; }
            public SyntaxList<AttributeListSyntax> Attributes{ get; set; }
            public ISingleTarget<InterfaceDeclarationSyntax> SingleTarget { get; set; }
            public SyntaxToken Modifiers { get; set; }
            public SyntaxToken Partial { get; set; }
            public string[] ImplementedInterfaces { get ; set ; }
        }

    }
}
