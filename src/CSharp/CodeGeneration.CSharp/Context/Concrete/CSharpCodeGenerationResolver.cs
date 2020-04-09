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
    public abstract partial class CSharpContext<TProcessEntity> : CodeGenContext<Project, CSharpSyntaxNode,CompilationUnitSyntax,ISymbol,TProcessEntity>
    {
        public class CSharpAutofacResolver : ICodeGenerationResolver
        {
            protected IContainer _container { get; set; }
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
                _container = builder.Build();
            }

            public TCommandBuilder ResolveCommandBuilder<TCommandBuilder,TSyntaxNode,TOutput>()
                where TCommandBuilder : ICommand<TSyntaxNode, TOutput>
                where TSyntaxNode : CSharpSyntaxNode
                where TOutput : CSharpSyntaxNode
            {
                return _container.Resolve<TCommandBuilder>();
            }

            public ICommandHandler<TCommand,TSyntaxNode, TOutput> ResolveCommandHandler<TCommand,TSyntaxNode,TOutput>(TCommand commandBuilder)
                where TCommand : ICommand<TSyntaxNode, TOutput>
                where TSyntaxNode : CSharpSyntaxNode
                where TOutput : CSharpSyntaxNode
            {
                return (ICommandHandler<TCommand,TSyntaxNode,TOutput>)_container.Resolve(typeof(ICommandHandler<TCommand, TSyntaxNode, TOutput>), new[] { new PositionalParameter(0, commandBuilder) });
            }
            
            protected void DoAutomaticRegister(ContainerBuilder builder)
            {
                var coreAssembly = Assembly.GetExecutingAssembly();

                ////Register Command Builders only as generic services
                foreach (var t in coreAssembly.GetTypes().Where(x => x.CustomAttributes.Any(a => a.AttributeType == typeof(CommandAttribute))))
                {
                    var abstractcommand = t.GetInterfaces().FirstOrDefault(i => i.IsAssignableTo<Core.ICommand>());
                    builder.RegisterGeneric(t).As(abstractcommand);
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