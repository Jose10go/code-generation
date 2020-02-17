using CodeGen.Core;

namespace CodeGen.Context
{
    public abstract partial class CodeGenContext<TProject, TRootNode, TSemanticModel, TProcessEntity>
    {
        public static ICodeGenerationResolver Resolver { get; set; }
        public interface ICodeGenerationResolver
        {
            void BuildContainer();
            TCommandBuilder ResolveCommandBuilder<TCommandBuilder,TSyntaxNode>()
                where TCommandBuilder :ICommand<TSyntaxNode>
                where TSyntaxNode:TRootNode;
            ICodeGenerationEngine ResolveEngine();
            void RegisterEngine(ICodeGenerationEngine engine);
            ICommandHandler<TSyntaxNode> ResolveCommandHandler<TSyntaxNode>(ICommand<TSyntaxNode> commandBuilder)
                where TSyntaxNode : TRootNode;
        }
    }
}
