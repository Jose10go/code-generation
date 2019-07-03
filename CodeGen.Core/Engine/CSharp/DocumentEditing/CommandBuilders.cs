using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Editing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeGen.Engine.CSharp.DocumentEditing
{
    public class MethodCloneCommandBuilder : CSharpCloneCommandBuilder<MethodDeclarationSyntax, DocumentEditor> { }

    public class ClassCloneCommandBuilder : CSharpCloneCommandBuilder<ClassDeclarationSyntax, DocumentEditor> { }
}
 