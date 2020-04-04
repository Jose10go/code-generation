namespace CodeGen.Context
{
    public abstract partial class CodeGenContext<TProject, TBaseNode, TRootNode, TSemanticModel, TProcessEntity>
        where TRootNode:TBaseNode
    {
        public interface ICodeGenerationResolver
        {
            void BuildContainer();
            void RegisterEngine(ICodeGenerationEngine engine);
            TCommandBuilder ResolveCommandBuilder<TCommandBuilder,TSyntaxNode>()
                where TCommandBuilder :ICommand<TSyntaxNode>
                where TSyntaxNode:TBaseNode;
            ICommandHandler<TCommand, TSyntaxNode> ResolveCommandHandler<TCommand,TSyntaxNode>(TCommand commandBuilder)
                where TCommand :ICommand<TSyntaxNode>
                where TSyntaxNode:TBaseNode;
        }
    }
}
