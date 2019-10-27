using CodeGen.Commands.CommandBuilders;
using System.Linq;
using static CodeGen.Context.CodeGenTypelessContext;
using CodeGen.Attributes;
namespace CodeGen.Context
{

    public partial class CodeGenContext<TProject, TRootNode, TProcessEntity>
    {
        public interface ICommandBuilder<TCommand> : ICommandBuilder
            where TCommand : ICommand, new()
        {

            TCommand Build()
            {
                var cmd = new TCommand();
                cmd.Target = this.Target;
                foreach (var modifier in this.GetType().GetInterfaces().Where(x=>x.CustomAttributes.Any(a=>a.AttributeType==typeof(CommandBuilderModifierAttribute))))
                    foreach (var prop in modifier.GetProperties())
                        cmd.With(prop.Name, prop.GetValue(this));
                return cmd;
            }

            void Go(TProcessEntity processEntity)
            {
                var command = Build();
                var handler = Resolver.ResolveCommandHandler<TCommand>();
                handler.Command = command;
                handler.ProcessDocument(processEntity);
            }
        }
    }
}
