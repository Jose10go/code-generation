using System;
using System.Linq;
using System.Reflection;
using Autofac;
using CodeGen.Attributes;
using CodeGen.Core;

namespace CodeGen.Context
{
    public abstract partial class CodeGenContext<TProject, TRootNode, TProcessEntity>
    {
        public abstract class AutofacResolver: ICodeGenerationResolver
        {
            protected IContainer _container { get; set; }
            protected ICodeGenerationEngine _engine;
            protected AutofacResolver()
            {
                Resolver = this;
            }
            public virtual void BuildContainer()
            {
                var builder = new ContainerBuilder();
                DoAutomaticRegister(builder);
                _container = builder.Build();
            }

            public void RegisterEngine(ICodeGenerationEngine engine)
            {
                _engine = engine;
            }

            public ICodeGenerationEngine ResolveEngine()
            {
                return _engine;
            }

            public ChainTargetBuilder<TNode> ResolveTargetBuilder<TNode>()
                where TNode:TRootNode
            {
                return _container.Resolve<ChainTargetBuilder<TNode>>();
            }

            protected virtual void DoAutomaticRegister(ContainerBuilder builder)
            {
                var coreAssembly = Assembly.GetExecutingAssembly();

                //Register Command Builders only as generic services
                foreach (var t in coreAssembly.GetTypes().Where(x => x.CustomAttributes.Any(a=>a.AttributeType==typeof(CommandBuilderAttribute))))
                    foreach (var i in t.GetInterfaces().Where(x=>x.IsGenericType))
                            builder.RegisterGeneric(t).As(i);

                //Register Command Handlers only as generic services
                foreach (var t in coreAssembly.GetTypes().Where(x => x.CustomAttributes.Any(a => a.AttributeType == typeof(CommandHandlerAttribute))))
                    foreach (var i in t.GetInterfaces().Where(x => x.IsGenericType))
                            builder.RegisterGeneric(t).As(i);

                // register the engine as singleton
                builder.RegisterInstance(_engine).As<ICodeGenerationEngine>().ExternallyOwned();
            }

            public ITarget<TSyntaxNode> ResolveTarget<TSyntaxNode>()
            {
                return _container.Resolve<ITarget<TSyntaxNode>>();
            }

            public TCommandBuilder ResolveCommandBuilder<TCommandBuilder,TSyntaxNode>()
                where TCommandBuilder : ICommandBuilder<TSyntaxNode>
                where TSyntaxNode:TRootNode
            {
                return _container.Resolve<TCommandBuilder>();
            }

            public ICommandHandler<TSyntaxNode> ResolveCommandHandler<TSyntaxNode>(ICommandBuilder<TSyntaxNode> commandBuilder)
                where TSyntaxNode : TRootNode
            {
                var cmdbuildertype = commandBuilder.GetType().GetInterfaces().First();
                var syntaxtype = typeof(TSyntaxNode);
                var handlertype = typeof(ICommandHandler<,>).MakeGenericType(new[] {typeof(TProject),typeof(TRootNode),typeof(TProcessEntity),cmdbuildertype,syntaxtype});
                return (ICommandHandler<TSyntaxNode>)_container.Resolve(handlertype, new[] { new PositionalParameter(0,commandBuilder)}) ;
            }

        }
    }
}
