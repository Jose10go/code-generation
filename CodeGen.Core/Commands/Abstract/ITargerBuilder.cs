using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeGen.Commands.Abstract
{
    public interface ITargetBuilder
    {
        Func<object, bool> WhereSelector { get; }
        ITargetBuilder Where(Func<object, bool> filter);
        ITarget Build();

        ICommandBuilder<TCommand> Execute<TCommand>() where TCommand : ICommand;
    }

    public interface ITargetBuilder<TNode, TRoot, TProcessEntity> : ITargetBuilder
    {
        new Func<TNode, bool> WhereSelector { get; set; }
        ITargetBuilder<TNode, TRoot, TProcessEntity> Where(Func<TNode, bool> filter);
        new ITarget<TNode, TRoot> Build();

        new ICommandBuilder<TCommand> Execute<TCommand>() where TCommand : ICommand<TNode, TRoot, TProcessEntity>;
    }
}
