using CodeGen.Context;
using CodeGen.Core.Attributes;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CodeGen.CSharp.Context
{
    public abstract partial class CSharpContext : CodeGenContext<Project, CSharpSyntaxNode, CompilationUnitSyntax,ISymbol>
    {
        public interface ICreateClass: ICommand<NamespaceDeclarationSyntax, ClassDeclarationSyntax>,
                                       IWithName<ICreateClass>,
                                       IWithAttribute<ICreateClass>,
                                       IWithAccessModifier<ICreateClass>,
                                       IAbstract<ICreateClass>,
                                       IStatic<ICreateClass>,
                                       IPartial<ICreateClass>
        {
        }

        [Command]
        public class CreateClassCommand : ICreateClass
        {
            public CreateClassCommand():base()
            {
            }

            public string Name { get; set; }
            public SyntaxList<AttributeListSyntax> Attributes{ get; set; }
            public ISingleTarget<NamespaceDeclarationSyntax> SingleTarget { get; set; }
            public SyntaxToken Modifiers { get; set; }
            public SyntaxToken Abstract { get; set; }
            public SyntaxToken Static { get; set; }
            public SyntaxToken Partial { get; set; }
        }

    }
}
