using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CodeGen.Commands.Abstract
{
    public interface ITarget<TNode, TRoot> : ITarget
    {
        new Func<TNode, bool> WhereSelector { get; }

        IEnumerable<TNode> Select(TRoot root);
    }


    public interface ITarget
    {
        Type TargetNode { get; }

        Func<object, bool> WhereSelector { get; }

        IEnumerable<object> Select(object root);
    }
}