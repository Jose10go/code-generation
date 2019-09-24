using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Editing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeGen.Context.CSharp
{
    public partial class ICSharpContext<TProcessEntity> : CodeGen.Context.CodeGenContext<Solution, CSharpSyntaxNode, TProcessEntity>
    {
        public class MethodCloneCommandBuilder : CloneCommandBuilder<MethodDeclarationSyntax> { }

        public class ClassCloneCommandBuilder : CloneCommandBuilder<ClassDeclarationSyntax> { }
    }
}
 