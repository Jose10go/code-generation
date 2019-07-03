using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CodeGen.Commands.Abstract;
using Microsoft.CodeAnalysis.CSharp;

namespace CodeGen.Engine.CSharp.Abstract
{
    interface ICSharpTarget<TNode> : ITarget<TNode, CSharpSyntaxNode>
        where TNode: CSharpSyntaxNode
    {
    }
}
