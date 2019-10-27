using static CodeGen.Context.CodeGenTypelessContext;

namespace CodeGen.Context
{
    public partial class CodeGenContext<TProject, TRootNode, TProcessEntity>
    {
        public static ICodeGenerationResolver Resolver { get; set; }
        public interface ICodeGenerationResolver
        {
            void BuildContainer();
            IChainTargetBuilder<TNode> ResolveTargetBuilder<TNode>();
            ITarget<TSyntaxNode> ResolveTarget<TSyntaxNode>();
            TCommandBuilder ResolveCommandBuilder<TCommandBuilder>()
            where TCommandBuilder :ICommandBuilder;
            ICodeGenerationEngine ResolveEngine();
            void RegisterEngine(ICodeGenerationEngine engine);
            ICommandHandler<TCommand> ResolveCommandHandler<TCommand>()
            where TCommand : ICommand;
        }
    }
}
