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
            ICommandBuilder<TCommand> ResolveCommandBuilder<TCommand>()
            where TCommand :ICommand,new();
            ICodeGenerationEngine ResolveEngine();
            void RegisterEngine(ICodeGenerationEngine engine);
            ICommandHandler<TCommand> ResolveCommandHandler<TCommand>()
            where TCommand : ICommand;
        }
    }
}
