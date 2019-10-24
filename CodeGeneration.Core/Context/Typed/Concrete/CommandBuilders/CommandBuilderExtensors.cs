using CodeGen.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static CodeGen.Context.CodeGenTypelessContext;

namespace CodeGen.Commands.CommandBuilders
{
    public static class CommandBuilderExtensions
    {
        public static void With<TCommand>
            (this TCommand commandBuilder, string prop, object value)
            where TCommand:ICommand
        {
            try
            {
                var property = commandBuilder.GetType().GetProperty(prop);
                property.SetValue(commandBuilder, value);
            }
            catch (Exception)
            {
                throw new InvalidOperationException($"Provided {nameof(prop)} doesn't exist in current CommandBuilder and underlying Command");
            }

        }

    }
}
