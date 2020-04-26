using Autofac;
using CodeGen.Attributes;
using CodeGen.Context;
using CodeGen.Core;
using CodeGen.Core.Attributes;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.Linq;
using System.Reflection;

namespace CodeGen.CSharp.Context
{
    public abstract partial class CSharpContext : CodeGenContext<Project, CSharpSyntaxNode,CompilationUnitSyntax,ISymbol>
    {
        public class CSharpAutofacResolver : ICodeGenerationResolver
        {
            protected IContainer container { get; set; }
            private readonly ContainerBuilder builder;
            
            public CSharpAutofacResolver()
            {
                builder = new ContainerBuilder();
            }

            public void RegisterEngine(ICodeGenerationEngine engine) 
            {
                builder.RegisterInstance(engine).As<ICodeGenerationEngine>();
            }

            public virtual void BuildContainer()
            {
                DoAutomaticRegister(builder);
                container = builder.Build();
            }

            public TCommand ResolveCommand<TCommand>()
                where TCommand : Core.ICommand
            {
                return container.Resolve<TCommand>();
            }

            public ICommandHandler<TCommand> ResolveCommandHandler<TCommand>(TCommand commandBuilder)
                where TCommand: Core.ICommand
            {
                return (ICommandHandler<TCommand>)container.Resolve(typeof(ICommandHandler<TCommand>), new[] { new PositionalParameter(0, commandBuilder) });
            }
            
            protected void DoAutomaticRegister(ContainerBuilder builder)
            {
                var coreAssembly = Assembly.GetExecutingAssembly();

                foreach (var t in coreAssembly.GetTypes().Where(x => x.CustomAttributes.Any(a => a.AttributeType == typeof(CommandAttribute))))
                {
                    var abstractcommand = t.GetInterfaces().FirstOrDefault(i => i.IsAssignableTo<Core.ICommand>());
                    if (t.IsGenericType)
                        builder.RegisterGeneric(t).As(abstractcommand);
                    else
                        builder.RegisterType(t).As(abstractcommand);
                }

                foreach (var t in coreAssembly.GetTypes().Where(x => x.CustomAttributes.Any(a => a.AttributeType == typeof(CommandHandlerAttribute))))
                {
                    var abstracthandler = t.GetInterfaces().FirstOrDefault(i => i.IsAssignableTo<Core.ICommandHandler>());
                    if (t.IsGenericType)
                        builder.RegisterGeneric(t).As(abstracthandler);
                    else 
                        builder.RegisterType(t).As(abstracthandler);
                }

            }

        }
    }
}