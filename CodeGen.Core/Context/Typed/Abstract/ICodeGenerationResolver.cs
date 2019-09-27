namespace CodeGen.Context
{
    public partial class CodeGenContext<TProject, TRootNode, TProcessEntity>
    {
        public static ICodeGenerationResolver Resolver { get; set; }
        public interface ICodeGenerationResolver
        {
            void BuildContainer();
            ITargetBuilder<TNode> ResolveTargetBuilder<TNode>();
            ICommandBuilder<TCommand, TNode> ResolveCommandBuilder<TCommand, TNode>()
            where TCommand :ICommand<TNode>;
            ICodeGenerationEngine ResolveEngine();
            void RegisterEngine(ICodeGenerationEngine engine);

            ICommandHandler<TCommand, ITarget<TNode>, TNode> ResolveCommandHandler<TCommand, TNode>()
            where TCommand : ICommand<TNode>;
        }
    }
}
