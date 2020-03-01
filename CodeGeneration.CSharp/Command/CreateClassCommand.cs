using CodeGen.Context;
using CodeGen.Core.Attributes;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CodeGen.CSharp.Context
{
    public abstract partial class CSharpContext<TProcessEntity> : CodeGenContext<Project, CSharpSyntaxNode, CompilationUnitSyntax,ISymbol, TProcessEntity>
    {
        public interface ICreateClass : ICommand<NamespaceDeclarationSyntax,ClassDeclarationSyntax>,
                                       IWithName<ICreateClass>,
                                       IWithAttribute<ICreateClass, ClassDeclarationSyntax>,
                                       IWithAccessModifier<ICreateClass, ClassDeclarationSyntax>,
                                       IAbstract<ICreateClass, ClassDeclarationSyntax>,
                                       IStatic<ICreateClass, ClassDeclarationSyntax>,
                                       IPartial<ICreateClass, ClassDeclarationSyntax>
        {
        }

        [Command]
        public class CreateClassCommand : CSharpTarget<ClassDeclarationSyntax>, ICreateClass
        {
            public CreateClassCommand(ICodeGenerationEngine engine):base(engine)
            {
            }

            public string Name { get; set; }
            public SyntaxList<AttributeListSyntax> Attributes{ get; set; }
            public Target<NamespaceDeclarationSyntax> Target { get; set; }
            public SyntaxToken Modifiers { get; set; }
            public SyntaxToken Abstract { get; set; }
            public SyntaxToken Static { get; set; }
            public SyntaxToken Partial { get; set; }
        }

    }
}
