using CodeGen.Commands;
using CodeGen.Engine.Abstract;
using CodeGen.Engine.CSharp.DocumentEditing;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeGen.Engine.CSharp
{
    public interface ICSharpCodeGenerationEngine<TProcessEntity> : ICodeGenerationEngine<Solution, CSharpSyntaxNode, TProcessEntity>
    {
        new ICSharpTargetBuilder<TSyntaxNode, TProcessEntity> Select<TSyntaxNode>() where TSyntaxNode: CSharpSyntaxNode;
    }
}
