using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CodeGen.Commands.Abstract
{
    public interface ICommand
    {
        ITarget Target { get; }
        ICommandHandler Handler { get; }
    }

    public interface ICommand<TSyntaxNode, TRootNode, TProcessEntity> : ICommand
    {
        new ITarget<TSyntaxNode, TRootNode> Target { get; set; }
    }
}