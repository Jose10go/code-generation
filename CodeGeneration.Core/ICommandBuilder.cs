using CodeGen.Attributes;
using System;
using System.Linq;
namespace CodeGen.Core
{
    public interface ICommandBuilder
    {
        ITarget Target { get; set; }
        Command Build()
        {
            var Command = new Command();
            Command.Target = this.Target;
            dynamic cmd = Command;
            foreach (var modifier in this.GetType().GetInterfaces().Where(x => x.CustomAttributes.Any(a => a.AttributeType == typeof(CommandBuilderModifierAttribute))))
                foreach (var prop in modifier.GetProperties())
                    cmd[$"{prop.Name}"] = prop.GetValue(this);
            return Command;
        }

    }
}
