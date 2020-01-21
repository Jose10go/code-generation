namespace CodeGen.Context
{
    public partial class CodeGenContext<TProject, TRootNode, TProcessEntity>
    {
        public interface ICommandBuilder : Core.ICommandBuilder
        {
            void Go()
            {
                var command = Build();
                var handler = Resolver.ResolveCommandHandler(this);
                handler.Command = command;
                handler.Command.Target.CodeGenerationEngine.ApplyChanges(handler);
            }
        }
    }
}