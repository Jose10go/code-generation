using CodeGen.Context;
using CodeGen.Core.Attributes;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CodeGen.CSharp.Context
{
    public abstract partial class CSharpContext<TProcessEntity> : CodeGenContext<Project, CSharpSyntaxNode, CompilationUnitSyntax,ISymbol, TProcessEntity>
    {
        public interface ICreateMethod : ICommand<ClassDeclarationSyntax,MethodDeclarationSyntax>,
                                         IWithName<ICreateMethod>,
                                         IWithAttribute<ICreateMethod, MethodDeclarationSyntax>,
                                         IWithBody<ICreateMethod, MethodDeclarationSyntax>,
                                         IWithAccessModifier<ICreateMethod, MethodDeclarationSyntax>,
                                         IAbstract<ICreateMethod, MethodDeclarationSyntax>,
                                         IStatic<ICreateMethod, MethodDeclarationSyntax>,
                                         IPartial<ICreateMethod, MethodDeclarationSyntax>
        {
        }

        [Command]
        public class CreateMethodCommand : ICreateMethod
        {
            public CreateMethodCommand():base()
            {
            }

            public string Name { get; set; }
            public SyntaxList<AttributeListSyntax> Attributes{ get; set; }
            public ITarget<ClassDeclarationSyntax> Target { get; set; }
            public SyntaxToken Modifiers { get; set; }
            public SyntaxToken Abstract { get; set; }
            public SyntaxToken Static { get; set; }
            public SyntaxToken Partial { get; set; }
            public BlockSyntax Body { get ; set ; }
        }

    }
}
