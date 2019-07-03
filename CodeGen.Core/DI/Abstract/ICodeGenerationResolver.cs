using CodeGen.Commands.Abstract;
using CodeGen.Engine.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeGen.DI.Abstract
{
    public interface ICodeGenerationResolver<TSolution, TRoot, TProcessEntity>
    {
        void BuildContainer();
        ITargetBuilder<TNode, TRoot, TProcessEntity> ResolveTargetBuilder<TNode>();
        ICommandBuilder<TCommand, TNode, TRoot, TProcessEntity> ResolveCommandBuilder<TCommand, TNode>() where TCommand : ICommand<TNode, TRoot, TProcessEntity>;

        ICodeGenerationEngine<TSolution, TRoot, TProcessEntity> ResolveEngine();
    }
}
