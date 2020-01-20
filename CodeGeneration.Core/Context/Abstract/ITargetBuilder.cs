using CodeGen.Core;
using System;

namespace CodeGen.Context
{
    public abstract partial class CodeGenContext<TProject, TRootNode, TProcessEntity> 
    {
        public interface ITargetBuilder<TNode> : ITargetBuilder
        {
            ICodeGenerationEngine Engine { get; set; }
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
            where TCommandBuilder : Core.ICommandBuilder
            {
                var target = Build();
                target.CodeGenerationEngine = Engine;
                var commandbuilder = Resolver.ResolveCommandBuilder<TCommandBuilder>();
                commandbuilder.Target = target;
                return commandbuilder;
            }
        }
    }
}
