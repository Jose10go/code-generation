using CodeGen.Context;
using CodeGen.Core.Attributes;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CodeGen.CSharp.Context
{
    public abstract partial class CSharpContext : CodeGenContext<Project, CSharpSyntaxNode, CompilationUnitSyntax,ISymbol>
{
        public interface ICloneMethod : ICommand<MethodDeclarationSyntax,MethodDeclarationSyntax>,
                                        IGet<ICloneMethod,MethodDeclarationSyntax>,
                                        IWithName<ICloneMethod>,
                                        IWithAttribute<ICloneMethod>,
                                        IWithBody<ICloneMethod>,
                                        IWithAccessModifier<ICloneMethod>,
                                        IAbstract<ICloneMethod>,
                                        IStatic<ICloneMethod>,
                                        IPartial<ICloneMethod>
        {
        }

        [Command]
        public class CloneMethodCommand : ICloneMethod
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
 