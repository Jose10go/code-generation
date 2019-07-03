﻿using CodeGen.Commands.Abstract;
using CodeGen.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeGen.Commands.CommandBuilders
{
    public static class CommandBuilderExtensions
    {
        public static CodeGenTypelessContext.ICommandBuilder<TCommand> With<TCommand, TValue>
            (this CodeGenTypelessContext.ICommandBuilder<TCommand> commandBuilder, string prop, TValue value)
            where TCommand: CodeGenTypelessContext.ICommand
        {
            try
            {
                var property = commandBuilder.GetType().GetProperty("With" + prop);

                property.SetValue(commandBuilder, value);
            }
            catch (Exception)
            {
                throw new InvalidOperationException($"Provided {nameof(prop)} doesn't exist in current CommandBuilder and underlying Command");
            }

            return commandBuilder;
        }

        public static CodeGenContext<TSolution, TRoot, TProcessEntity>.ICommandBuilder<TCommand, TNode> With<TCommand, TSolution, TRoot, TValue, TNode, TProcessEntity>
            (this CodeGenContext<TSolution, TRoot, TProcessEntity>.ICommandBuilder<TCommand, TNode> commandBuilder, string prop, TValue value)
            where TCommand: CodeGenContext<TSolution, TRoot, TProcessEntity>.ICommand<TNode>
        {
            try
            {
                var property = commandBuilder.GetType().GetProperty("With" + prop);

                property.SetValue(commandBuilder, value);
            }
            catch (Exception e)
            {
                throw new InvalidOperationException($"Provided {nameof(prop)} doesn't exist in current CommandBuilder and underlying Command");
            }

            return commandBuilder;
        }
    }
}
