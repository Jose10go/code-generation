using CodeGen.Attributes;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CodeGen.Context.CSharp
{
    public partial class CSharpContext<TProcessEntity> : CodeGenContext<Solution, CSharpSyntaxNode, TProcessEntity>
    {
        [CommandBuilder]
        public class MethodCloneCommandBuilder : CloneCommandBuilder<MethodDeclarationSyntax> { }

        [CommandBuilder]
        public class ClassCloneCommandBuilder : CloneCommandBuilder<ClassDeclarationSyntax> { }
    }
}
 