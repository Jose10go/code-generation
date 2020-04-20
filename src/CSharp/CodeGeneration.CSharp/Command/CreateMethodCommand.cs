using CodeGen.Context;
using CodeGen.Core.Attributes;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CodeGen.CSharp.Context
{
    public abstract partial class CSharpContext : CodeGenContext<Project, CSharpSyntaxNode, CompilationUnitSyntax,ISymbol>
    {
        public interface ICreateMethod : ICommand<ClassDeclarationSyntax,MethodDeclarationSyntax>,
                                         IWithName<ICreateMethod>,
                                         IWithAttribute<ICreateMethod>,
                                         IWithBody<ICreateMethod>,
                                         IWithAccessModifier<ICreateMethod>,
                                         IAbstract<ICreateMethod>,
                                         IStatic<ICreateMethod>,
                                         IPartial<ICreateMethod>,
                                         IReturns<ICreateMethod>
        {
        }

        [Command]
        public class CreateMethodCommand : ICreateMethod
        {
            public CreateMethodCommand():base()
            {
            }

            public string Name { get; set; }
            public string ReturnType { get; set; }
            public SyntaxList<AttributeListSyntax> Attributes{ get; set; }
            public ISingleTarget<ClassDeclarationSyntax> SingleTarget { get; set; }
            public SyntaxToken Modifiers { get; set; }
            public SyntaxToken Abstract { get; set; }
            public SyntaxToken Static { get; set; }
            public SyntaxToken Partial { get; set; }
            public BlockSyntax Body { get ; set ; }
        }

    }
}
