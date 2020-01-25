using CodeGen.Attributes;
using System.Linq;

namespace CodeGen.Context
{
    public partial class CodeGenContext<TProject, TRootNode, TProcessEntity>
    {
        public interface ICommandBuilder<TSyntaxNode> : Core.ICommandBuilder
            where TSyntaxNode:TRootNode
        {
            ITarget<TSyntaxNode> Target { get; set; }

            void Go()
            {
                var command = Build();
                var handler = Resolver.ResolveCommandHandler(this);
                handler.Command = command;
                handler.Command.Target.CodeGenerationEngine.ApplyChanges(handler);
            }

            Command<TSyntaxNode> Build()
            {
                var Command = new Command<TSyntaxNode>();
                Command.Target = this.Target;
                dynamic cmd = Command;
                foreach (var modifier in this.GetType().GetInterfaces().Where(x => x.CustomAttributes.Any(a => a.AttributeType == typeof(CommandBuilderModifierAttribute))))
                    foreach (var prop in modifier.GetProperties())
                        cmd[$"{prop.Name}"] = prop.GetValue(this);
                return Command;
            }

        }
    }
}