using CodeGen.Context;
using CodeGen.Core.Attributes;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CodeGen.CSharp.Context
{
    public abstract partial class CSharpContext : CodeGenContext<Project, CSharpSyntaxNode, CompilationUnitSyntax,ISymbol>
{
        public interface IMethodClone : ICommand<MethodDeclarationSyntax,MethodDeclarationSyntax>,
                                        IGet<IMethodClone,MethodDeclarationSyntax>,
                                        IWithName<IMethodClone>,
                                        IWithAttribute<IMethodClone>,
                                        IWithBody<IMethodClone>,
                                        IWithAccessModifier<IMethodClone>,
                                        IAbstract<IMethodClone>,
                                        IStatic<IMethodClone>,
                                        IPartial<IMethodClone>
        {
        }

        [Command]
        public class MethodCloneCommand : IMethodClone
        {
            public string Name { get; set; }
            public SyntaxList<AttributeListSyntax> Attributes{ get; set; }
            public BlockSyntax Body { get; set; }
            public ISingleTarget<MethodDeclarationSyntax> SingleTarget { get; set ; }
            public SyntaxToken Modifiers { get ; set ; }
            public SyntaxToken Abstract { get ; set ; }
            public SyntaxToken Static { get ; set ; }
            public SyntaxToken Partial { get ; set ; }
        }
        
    }
}
 