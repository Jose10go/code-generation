using CodeGen.Context;
using CodeGen.Core;
using CodeGen.Core.Attributes;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CodeGen.CSharp.Context
{
    public abstract partial class CSharpContext : CodeGenContext<Project, CSharpSyntaxNode, CompilationUnitSyntax,ISymbol>
{
        public interface IModifyMethod :ICommandResult<MethodDeclarationSyntax>,
                                        ICommandOn<MethodDeclarationSyntax>,
                                        IGet<IModifyMethod>,
                                        IWithName<IModifyMethod>,
                                        IWithAttribute<IModifyMethod>,
                                        IWithBody<IModifyMethod>,
                                        IWithAccessModifier<IModifyMethod>,
                                        IAbstract<IModifyMethod>,
                                        IStatic<IModifyMethod>,
                                        IPartial<IModifyMethod>
        {
        }

        [Command]
        public class ModifyMethodCommand : IModifyMethod
        {
            public string Name { get; set; }
            public SyntaxList<AttributeListSyntax> Attributes{ get; set; }
            public BlockSyntax Body { get; set; }
            public ISingleTarget SingleTarget { get; set ; }
            public SyntaxToken Modifiers { get ; set ; }
            public SyntaxToken Abstract { get ; set ; }
            public SyntaxToken Static { get ; set ; }
            public SyntaxToken Partial { get ; set ; }
        }
        
    }
}
 