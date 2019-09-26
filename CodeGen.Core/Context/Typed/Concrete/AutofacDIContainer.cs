using System;
using System.Linq;
using System.Reflection;
using Autofac;

namespace CodeGen.Context
{
    public partial class CodeGenContext<TProject, TRootNode, TProcessEntity>
    {
        public class AutofacResolver: ICodeGenerationResolver
        {
            protected IContainer _container { get; set; }
            protected ICodeGenerationEngine _engine;

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

            protected virtual void DoAutomaticRegister(ContainerBuilder builder)
            {
                var coreAssembly = Assembly.GetExecutingAssembly();

                foreach (var t in coreAssembly.GetTypes().Where(x => x.Name.EndsWith("CommandBuilder")))
                    builder.RegisterGeneric(t).AsImplementedInterfaces();

                foreach (var t in coreAssembly.GetTypes().Where(x =>x.IsClass && x.Name.EndsWith("Command")))
                    builder.RegisterGeneric(t).AsImplementedInterfaces();
            
                // register the engine as singleton
                builder.RegisterInstance(_engine).As<ICodeGenerationEngine>().ExternallyOwned();
                // register this as singleton
                builder.RegisterInstance(this).As<ICodeGenerationResolver>().ExternallyOwned();

            }
        }
    }
}
