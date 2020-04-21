using CodeGen.Context;
using CodeGen.Core.Attributes;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CodeGen.CSharp.Context
{
    public abstract partial class CSharpContext : CodeGenContext<Project, CSharpSyntaxNode, CompilationUnitSyntax,ISymbol>
    {
        public interface IModifyClass : ICommand<ClassDeclarationSyntax,ClassDeclarationSyntax>,
                                       IGet<IModifyClass,ClassDeclarationSyntax>,
                                       IWithName<IModifyClass>,
                                       IWithAttribute<IModifyClass>,
                                       IWithAccessModifier<IModifyClass>,
                                       IAbstract<IModifyClass>,
                                       IStatic<IModifyClass>,
                                       IPartial<IModifyClass>,
                                       IInheritsFrom<IModifyClass>,
                                       IImplements<IModifyClass>
        {
        }

        [Command]
        public class ModifyClassCommand : IModifyClass
        {
            public ModifyClassCommand():base()
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
