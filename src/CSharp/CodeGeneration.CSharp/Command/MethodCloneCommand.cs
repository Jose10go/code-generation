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
        public interface IMethodClone : ICommand<MethodDeclarationSyntax, MethodDeclarationSyntax>,
                                        IWithNewName<IMethodClone,MethodDeclarationSyntax>,
                                        IWithAttribute<IMethodClone, MethodDeclarationSyntax>,
                                        IWithBody<IMethodClone,MethodDeclarationSyntax>,
                                        IWithAccessModifier<IMethodClone,MethodDeclarationSyntax>,
                                        IAbstract<IMethodClone,MethodDeclarationSyntax>,
                                        IStatic<IMethodClone, MethodDeclarationSyntax>,
                                        IPartial<IMethodClone,MethodDeclarationSyntax>
        {
        }

        [Command]
        public class MethodCloneCommand : CSharpMultipleTarget<MethodDeclarationSyntax>, IMethodClone
        {
            public Func<MethodDeclarationSyntax, string> NewName { get; set; }
            public SyntaxList<AttributeListSyntax> Attributes{ get; set; }
            public BlockSyntax Body { get; set; }
            public ITarget<MethodDeclarationSyntax> Target { get; set ; }
            public SyntaxToken Modifiers { get ; set ; }
            public SyntaxToken Abstract { get ; set ; }
            public SyntaxToken Static { get ; set ; }
            public SyntaxToken Partial { get ; set ; }

            public MethodCloneCommand(ICodeGenerationEngine engine):base(engine)
            {

            }
        }
        
    }
}
 