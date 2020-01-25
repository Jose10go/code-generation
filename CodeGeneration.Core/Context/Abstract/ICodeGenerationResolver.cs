using CodeGen.Core;

namespace CodeGen.Context
{
    public abstract partial class CodeGenContext<TProject, TRootNode, TProcessEntity>
    {
        public static ICodeGenerationResolver Resolver { get; set; }
        public interface ICodeGenerationResolver
        {
            void BuildContainer();
            ChainTargetBuilder<TNode> ResolveTargetBuilder<TNode>()
                where TNode : TRootNode;
            ITarget<TSyntaxNode> ResolveTarget<TSyntaxNode>();
            TCommandBuilder ResolveCommandBuilder<TCommandBuilder,TSyntaxNode>()
                where TCommandBuilder :ICommandBuilder<TSyntaxNode>
                where TSyntaxNode:TRootNode;
            ICodeGenerationEngine ResolveEngine();
            void RegisterEngine(ICodeGenerationEngine engine);
            ICommandHandler<TSyntaxNode> ResolveCommandHandler<TSyntaxNode>(ICommandBuilder<TSyntaxNode> commandBuilder)
                where TSyntaxNode : TRootNode;
        }
    }
}
