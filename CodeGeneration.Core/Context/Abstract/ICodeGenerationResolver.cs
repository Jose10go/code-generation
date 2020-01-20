﻿using CodeGen.Core;

namespace CodeGen.Context
{
    public abstract partial class CodeGenContext<TProject, TRootNode, TProcessEntity>
    {
        public static ICodeGenerationResolver Resolver { get; set; }
        public interface ICodeGenerationResolver
        {
            void BuildContainer();
            IChainTargetBuilder<TNode> ResolveTargetBuilder<TNode>();
            ITarget<TSyntaxNode> ResolveTarget<TSyntaxNode>();
            TCommandBuilder ResolveCommandBuilder<TCommandBuilder>()
            where TCommandBuilder :Core.ICommandBuilder;
            ICodeGenerationEngine ResolveEngine();
            void RegisterEngine(ICodeGenerationEngine engine);
            TCommandHandler ResolveCommandHandler<TCommandHandler>()
            where TCommandHandler : Core.ICommandHandler;
        }
    }
}
