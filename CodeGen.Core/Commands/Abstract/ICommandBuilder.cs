using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CodeGen.Commands.Abstract
{
    public interface ICommandBuilder<TCommand>
        where TCommand : ICommand
    {
        TCommand Build();
    }

    public interface ICommandBuilder<TCommand, TNode, TRoot, TProcessEntity> : ICommandBuilder<TCommand>
        where TCommand : ICommand<TNode, TRoot, TProcessEntity>
    {
        new TCommand Build();
    }
}