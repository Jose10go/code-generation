namespace CodeGen.Context
{
    public abstract partial class CodeGenContext<TProject, TBaseNode, TRootNode, TSemanticModel>
        where TRootNode:TBaseNode
    {
        public interface ICodeGenerationResolver
        {
            void BuildContainer();
            void RegisterEngine(ICodeGenerationEngine engine);
            TCommand ResolveCommand<TCommand>()
                where TCommand : Core.ICommand;
            ICommandHandler<TCommand> ResolveCommandHandler<TCommand>(TCommand commandBuilder)
                where TCommand : Core.ICommand;
        }
    }
}
