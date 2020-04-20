namespace CodeGen.Context
{
    public abstract partial class CodeGenContext<TProject, TBaseNode, TRootNode, TSemanticModel>
        where TRootNode:TBaseNode
    {
        public interface ICodeGenerationResolver
        {
            void BuildContainer();
            void RegisterEngine(ICodeGenerationEngine engine);
            TCommandBuilder ResolveCommandBuilder<TCommandBuilder,TSyntaxNode,TOutputNode>()
                where TCommandBuilder :ICommand<TSyntaxNode,TOutputNode>
                where TSyntaxNode:TBaseNode
                where TOutputNode:TBaseNode;
            ICommandHandler<TCommand, TSyntaxNode,TOutputNode> ResolveCommandHandler<TCommand, TSyntaxNode, TOutputNode>(TCommand commandBuilder)
                where TCommand : ICommand<TSyntaxNode,TOutputNode>
                where TSyntaxNode : TBaseNode
                where TOutputNode : TBaseNode;
        }
    }
}
