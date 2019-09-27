using System;
using System.Linq;
using System.Reflection;
using Autofac;
using CodeGen.Attributes;

namespace CodeGen.Context
{
    public partial class CodeGenContext<TProject, TRootNode, TProcessEntity>
    {
        public class AutofacResolver: ICodeGenerationResolver
        {
            protected IContainer _container { get; set; }
            protected ICodeGenerationEngine _engine;
            public AutofacResolver()
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

            public ICommandBuilder<TCommand, TNode> ResolveCommandBuilder<TCommand, TNode>()
                where TCommand : ICommand<TNode>
            {
                return _container.Resolve<ICommandBuilder<TCommand, TNode>>();
            }

            public ICodeGenerationEngine ResolveEngine()
            {
                return _engine;
            }

            public ITargetBuilder<TNode> ResolveTargetBuilder<TNode>()
            {
                return _container.Resolve<ITargetBuilder<TNode>>();
            }

            public ICommandHandler<TCommand, ITarget<TNode>, TNode> ResolveCommandHandler<TCommand, TNode>()
            where TCommand : ICommand<TNode>
            {
                return _container.Resolve<ICommandHandler<TCommand, ITarget<TNode>, TNode>>();
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
                        builder.RegisterType(t).As(i);

                // register the engine as singleton
                builder.RegisterInstance(_engine).As<ICodeGenerationEngine>().ExternallyOwned();
            }
        }
    }
}
