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
                where TCommandBuilder :ICommandBuilder<TSyntaxNode>
                where TSyntaxNode:TRootNode;
            ICodeGenerationEngine ResolveEngine();
            void RegisterEngine(ICodeGenerationEngine engine);
            ICommandHandler<TSyntaxNode> ResolveCommandHandler<TSyntaxNode>(ICommandBuilder<TSyntaxNode> commandBuilder)
                where TSyntaxNode : TRootNode;
        }
    }
}
