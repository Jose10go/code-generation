using CodeGen.Core;

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
            ICommandHandler ResolveCommandHandler(ICommand commandBuilder);
        }
    }
}
