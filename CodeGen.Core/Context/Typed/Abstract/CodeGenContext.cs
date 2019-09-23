using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeGen.Context
{
    // Typed context - Abstract
    public partial class CodeGenContext<TProject, TRootNode, TProcessEntity> : CodeGenTypelessContext
    {
        public interface ITarget<out TNode> : ITarget
        {
            //new Func<TNode, bool> WhereSelector { get; }//TODO check without func because of in Tnode

            IEnumerable<TNode> Select(TRootNode root);
        }

        public interface ICommand<out TSyntaxNode> : ICommand
        {
            new ITarget<TSyntaxNode> Target { get;}
        }

        public interface ICommandHandler<TCommand, out TTarget, TNode> : ICommandHandler
            where TCommand : ICommand<TNode>
            where TTarget : ITarget<TNode>
        {
            new TTarget Target { get; }

            new TCommand Command { get; }

            TProcessEntity ProcessDocument(TProcessEntity entity);
        }

        public interface ITargetBuilder<TNode> : ITargetBuilder
        {
            new Func<TNode, bool> WhereSelector { get; set; }
            ITargetBuilder<TNode> Where(Func<TNode, bool> filter);
            new ITarget<TRootNode> Build();

            new ICommandBuilder<TCommand> Execute<TCommand>() where TCommand : ICommand<TNode>;
        }

        public interface ICommandBuilder<TCommand, TNode> : ICommandBuilder<TCommand>
            where TCommand : ICommand<TNode>
        {
            new TCommand Build();
        }

        public new interface ICodeGenerationEngine : CodeGenTypelessContext.ICodeGenerationEngine
        {
            new TProject ApplyChanges();

            new TProject CurrentSolution { get; }

            new ITargetBuilder<TSyntaxNode> Select<TSyntaxNode>() where TSyntaxNode : TRootNode;
        }
    }
}
