using CodeGen.Commands.Abstract;
using Microsoft.CodeAnalysis.CSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeGen.Engine.CSharp
{
    public interface ICSharpTargetBuilder<TNode, TProcessEntity> : ITargetBuilder<TNode, CSharpSyntaxNode, TProcessEntity>
    {
    }
}
