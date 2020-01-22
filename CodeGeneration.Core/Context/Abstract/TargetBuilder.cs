using CodeGen.Core;
using System;

namespace CodeGen.Context
{
    public abstract partial class CodeGenContext<TProject, TRootNode, TProcessEntity> 
    {
        public abstract class TargetBuilder<TNode> : ITargetBuilder
        {
            protected TargetBuilder(ICodeGenerationEngine engine, ITarget<TNode> target)
            {
                Engine = engine;
                Target = target;
            }
            protected ICodeGenerationEngine Engine { get;}
            protected ITarget<TNode> Target { get; }
            public TargetBuilder<TNode> Where(Func<TNode, bool> filter)
            {
                Target.WhereSelector = filter;
                return this;
            }
            public ITarget<TNode> Build() 
            {
                return Target;
            }
        }

        public abstract class ChainTargetBuilder<TNode>
        {
            protected ChainTargetBuilder(ICodeGenerationEngine engine, ITarget<TNode> target)
            {
                Engine = engine;
                Target = target;
            }
            protected ICodeGenerationEngine Engine { get; }
            protected ITarget<TNode> Target { get; }

            public ChainTargetBuilder<TNode> Where(Func<TNode, bool> filter)
            {
                Target.WhereSelector = filter;
                return this;
            }
            public TCommandBuilder Execute<TCommandBuilder>()
            where TCommandBuilder : Core.ICommandBuilder
            {
                Target.CodeGenerationEngine = Engine;
                var commandbuilder = Resolver.ResolveCommandBuilder<TCommandBuilder>();
                commandbuilder.Target = Target;
                return commandbuilder;
            }
        }
    }
}
