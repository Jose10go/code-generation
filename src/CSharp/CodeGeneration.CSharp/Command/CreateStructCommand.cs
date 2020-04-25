using CodeGen.Context;
using CodeGen.Core.Attributes;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.Collections.Generic;

namespace CodeGen.CSharp.Context
{
    public abstract partial class CSharpContext : CodeGenContext<Project, CSharpSyntaxNode, CompilationUnitSyntax,ISymbol>
    {
        public interface ICreateStruct: ICommand<NamespaceDeclarationSyntax, StructDeclarationSyntax>,
                                        IGet<ICreateStruct,NamespaceDeclarationSyntax>,
                                        IWithName<ICreateStruct>,
                                        IWithAttribute<ICreateStruct>,
                                        IWithAccessModifier<ICreateStruct>,
                                        IPartial<ICreateStruct>,
                                        IImplements<ICreateStruct>,
                                        IWithGeneric<ICreateStruct>
        {
        }

        [Command]
        public class CreateStructCommand : ICreateStruct
        {
            public CreateStructCommand():base()
            {
            }

            public string Name { get; set; }
            public SyntaxList<AttributeListSyntax> Attributes{ get; set; }
            public ISingleTarget<NamespaceDeclarationSyntax> SingleTarget { get; set; }
            public SyntaxToken Modifiers { get; set; }
            public SyntaxToken Partial { get; set; }
            public string[] ImplementedInterfaces { get ; set ; }
            public Dictionary<string, List<string>> GenericTypes { get ; set ; }
        }

    }
}
