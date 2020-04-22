using CodeGen.Context;
using CodeGen.Core.Attributes;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CodeGen.CSharp.Context
{
    public abstract partial class CSharpContext : CodeGenContext<Project, CSharpSyntaxNode, CompilationUnitSyntax,ISymbol>
{
        public interface IModifyProperty : ICommand<PropertyDeclarationSyntax,PropertyDeclarationSyntax>,
                                        IGet<IModifyProperty,PropertyDeclarationSyntax>,
                                        IWithName<IModifyProperty>,
                                        IWithAttribute<IModifyProperty>,
                                        IWithAccessModifier<IModifyProperty>,
                                        IAbstract<IModifyProperty>,
                                        IStatic<IModifyProperty>
        {
        }

        [Command]
        public class ModifyPropertyCommand : IModifyProperty
        {
            public string Name { get; set; }
            public SyntaxList<AttributeListSyntax> Attributes{ get; set; }
            public ISingleTarget<PropertyDeclarationSyntax> SingleTarget { get; set ; }
            public SyntaxToken Modifiers { get ; set ; }
            public SyntaxToken Abstract { get ; set ; }
            public SyntaxToken Static { get ; set ; }
        }
        
    }
}
 