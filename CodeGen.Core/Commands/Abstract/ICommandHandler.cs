using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CodeGen.Commands.Abstract
{
    public interface ICommandHandler<TCommand, out TTarget, TNode, TRootNode, TProcessEntity> : ICommandHandler
        where TCommand: ICommand<TNode, TRootNode, TProcessEntity>
        where TTarget : ITarget<TNode, TRootNode>
    {
        new TTarget Target { get; }

        new TCommand Command { get; }

        TProcessEntity ProcessDocument(TProcessEntity entity);
    }

    public interface ICommandHandler
    {
        ITarget Target { get; }

        ICommand Command { get; }

        object ProcessDocument(object entity);
    }
}