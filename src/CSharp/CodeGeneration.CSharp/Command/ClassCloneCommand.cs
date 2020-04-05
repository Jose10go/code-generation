using CodeGen.Context;
using CodeGen.Core.Attributes;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System;

namespace CodeGen.CSharp.Context
{
    public abstract partial class CSharpContext<TProcessEntity> : CodeGenContext<Project, CSharpSyntaxNode, CompilationUnitSyntax,ISymbol, TProcessEntity>
    {
        public interface IClassClone : ICommand<ClassDeclarationSyntax, ClassDeclarationSyntax>,
                                       IWithNewName<IClassClone, ClassDeclarationSyntax>,
                                       IWithAttribute<IClassClone, ClassDeclarationSyntax>,
                                       IWithAccessModifier<IClassClone,ClassDeclarationSyntax>,
                                       IAbstract<IMethodClone, ClassDeclarationSyntax>,
                                       IStatic<IMethodClone, ClassDeclarationSyntax>,
                                       IPartial<IMethodClone, ClassDeclarationSyntax>
        {
        }

        [Command]
        public class ClassCloneCommand : CSharpMultipleTarget<ClassDeclarationSyntax>,IClassClone
        {
            public ClassCloneCommand(ICodeGenerationEngine engine):base(engine)
            {
            }

            public Func<ClassDeclarationSyntax, string> NewName { get; set; }
            public SyntaxList<AttributeListSyntax> Attributes{ get; set; }
            public ITarget<ClassDeclarationSyntax> Target { get; set; }
            public SyntaxToken Modifiers { get; set; }
            public SyntaxToken Abstract { get; set; }
            public SyntaxToken Static { get; set; }
            public SyntaxToken Partial { get; set; }
        }

    }
}
