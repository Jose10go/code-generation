using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeGen.Context
{
    // Abstract Typeless Context
    public partial class CodeGenTypelessContext
    {
        public interface ITarget
        {
            Type TargetNode { get; }

            Func<object, bool> WhereSelector { get; }

            IEnumerable<object> Select(object root);
        }

        public interface ICommandHandler
        {
            ITarget Target { get; }

            ICommand Command { get; }

            object ProcessDocument(object entity);
        }

        public interface ICommand
        {
            ITarget Target { get; }
            ICommandHandler Handler { get; }
        }

        public interface ICommandBuilder<TCommand> where TCommand : ICommand
        {
            TCommand Build();
        }

        public interface ITargetBuilder
        {
            ITargetBuilder Where(Func<object, bool> filter);
            ITarget Build();
            ICommandBuilder<TCommand> Execute<TCommand>() where TCommand : ICommand;
        }

        public interface ICodeGenerationEngine
        {
            object ApplyChanges();

            object CurrentSolution { get; }

            ITargetBuilder Select<TSyntaxNode>();
        }
    }
}
