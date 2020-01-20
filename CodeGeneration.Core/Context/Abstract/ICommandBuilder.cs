namespace CodeGen.Context
{
    public partial class CodeGenContext<TProject, TRootNode, TProcessEntity>
    {
        public interface ICommandBuilder : Core.ICommandBuilder
        {
            void Go<TCommandHandler>() where TCommandHandler : Core.ICommandHandler
            {
                var command = Build();
                var handler = Resolver.ResolveCommandHandler<TCommandHandler>();
                handler.Command = command;
                handler.Command.Target.CodeGenerationEngine.ApplyChanges(handler);
            }
        }
    }
}