using CodeGen.Attributes;
using CodeGen.Context;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System;

namespace CodeGen.CSharp.Context
{
    public abstract partial class CSharpContext<TProcessEntity> : CodeGenContext<Solution, CSharpSyntaxNode,ISymbol, TProcessEntity>
{
        public interface IMethodClone : ICommandBuilder<MethodDeclarationSyntax>,
                                        IWithNewName<IMethodClone,MethodDeclarationSyntax>,
                                        IWithAttribute<IMethodClone, MethodDeclarationSyntax>,
                                        IWithBody<IMethodClone,MethodDeclarationSyntax>,
                                        IMakePublic<IMethodClone,MethodDeclarationSyntax>,
                                        IMakePrivate<IMethodClone,MethodDeclarationSyntax>,
                                        IMakeProtected<IMethodClone,MethodDeclarationSyntax>,
                                        IMakeInternal<IMethodClone,MethodDeclarationSyntax>
        {
        }

        [CommandBuilder]
        public class MethodCloneCommandBuilder : IMethodClone
        {
            public Func<MethodDeclarationSyntax, string> NewName { get; set; }
            public SyntaxList<AttributeListSyntax> Attributes{ get; set; }
            public BlockSyntax Body { get; set; }
            public ITarget<MethodDeclarationSyntax> Target { get; set ; }
            public SyntaxTokenList Modifiers { get ; set ; }
        }
        
    }
}
 