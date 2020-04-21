using CodeGen.Context;
using CodeGen.Core.Attributes;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CodeGen.CSharp.Context
{
    public abstract partial class CSharpContext : CodeGenContext<Project, CSharpSyntaxNode, CompilationUnitSyntax,ISymbol>
    {
        public interface IClassClone : ICommand<ClassDeclarationSyntax,ClassDeclarationSyntax>,
                                       IGet<IClassClone,ClassDeclarationSyntax>,
                                       IWithName<IClassClone>,
                                       IWithAttribute<IClassClone>,
                                       IWithAccessModifier<IClassClone>,
                                       IAbstract<IClassClone>,
                                       IStatic<IClassClone>,
                                       IPartial<IClassClone>,
                                       IInheritsFrom<IClassClone>,
                                       IImplements<IClassClone>
        {
        }

        [Command]
        public class CloneClassCommand : IClassClone
        {
            public CloneClassCommand():base()
            {
            }

            public string Name { get; set; }
            public SyntaxList<AttributeListSyntax> Attributes{ get; set; }
            public ISingleTarget<ClassDeclarationSyntax> SingleTarget { get; set; }
            public SyntaxToken Modifiers { get; set; }
            public SyntaxToken Abstract { get; set; }
            public SyntaxToken Static { get; set; }
            public SyntaxToken Partial { get; set; }
            public string InheritsType { get ; set ; }
            public string[] ImplementedInterfaces { get ; set ; }
        }

    }
}
