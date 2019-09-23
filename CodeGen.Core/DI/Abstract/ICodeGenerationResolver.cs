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
        Context.CodeGenContext<TSolution, TRoot, TProcessEntity>.ITargetBuilder<TNode> ResolveTargetBuilder<TNode>();
        Context.CodeGenContext<TSolution, TRoot, TProcessEntity>.ICommandBuilder<TCommand, TNode> ResolveCommandBuilder<TCommand, TNode>()
        where TCommand : Context.CodeGenContext<TSolution, TRoot, TProcessEntity>.ICommand<TNode>;
        Context.CodeGenContext<TSolution, TRoot, TProcessEntity>.ICodeGenerationEngine ResolveEngine();
    }
}
