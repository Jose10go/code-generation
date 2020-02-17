using CodeGen.Core;

namespace CodeGen.Context
{
    public abstract partial class CodeGenContext<TProject, TRootNode, TSemanticModel, TProcessEntity>
    {
        public interface ICodeGenerationResolver
        {
            void BuildContainer();
            void RegisterEngine(ICodeGenerationEngine engine);
            TCommandBuilder ResolveCommandBuilder<TCommandBuilder,TSyntaxNode>()
                where TCommandBuilder :ICommand<TSyntaxNode>
                where TSyntaxNode:TRootNode;
            ICommandHandler ResolveCommandHandler(ICommand commandBuilder);
        }
    }
}
