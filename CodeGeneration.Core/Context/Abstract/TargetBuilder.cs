﻿using CodeGen.Core;
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
            where TNode:TRootNode
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
                where TCommandBuilder : ICommandBuilder<TNode>
            {
                Target.CodeGenerationEngine = Engine;
                var commandBuilder=Resolver.ResolveCommandBuilder<TCommandBuilder,TNode>();
                commandBuilder.Target = Target;
                return commandBuilder;
            }
        }
    }
}
