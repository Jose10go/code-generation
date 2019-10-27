using System;
using static CodeGen.Context.CodeGenTypelessContext;

namespace CodeGen.Context
{

    public partial class CodeGenContext<TProject, TRootNode, TProcessEntity> 
    {
        public interface ITargetBuilder<TNode> : ITargetBuilder
        {
            Func<TNode, bool> WhereSelector { get; set; }
            
            ITarget<TNode> Build() 
            {
                var target=Resolver.ResolveTarget<TNode>();
                target.WhereSelector = WhereSelector;
                return target;
            }
        }

        public interface IChainTargetBuilder<TNode>:ITargetBuilder<TNode>
        {
            IChainTargetBuilder<TNode> Where(Func<TNode, bool> filter)
            {
                WhereSelector = filter;
                return this;
            }
            TCommandBuilder Execute<TCommandBuilder>()
                where TCommandBuilder : ICommandBuilder
            {
                var target = Build();
                var commandbuilder = Resolver.ResolveCommandBuilder<TCommandBuilder>();
                commandbuilder.Target = target;
                return commandbuilder;
            }
        }
    }
}
