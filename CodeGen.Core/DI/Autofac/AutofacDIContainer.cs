using Microsoft.CodeAnalysis.Editing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Autofac;
using CodeGen.DI.Abstract;
using CodeGen.Engine.Abstract;
using CodeGen.Commands.Abstract;

namespace CodeGen.DI
{
    public class AutofacResolver<TSolution, TRoot, TProcessEntity> : ICodeGenerationResolver<TSolution, TRoot, TProcessEntity>
    {
        protected IContainer _container { get; set; }
        protected ICodeGenerationEngine<TSolution, TRoot, TProcessEntity> _engine;

        public virtual void BuildContainer()
        {
            var builder = new ContainerBuilder();

            _container = builder.Build();
        }

        public void RegisterEngine(ICodeGenerationEngine<TSolution, TRoot, TProcessEntity> engine)
        {
            _engine = engine;
        }

        public ICommandBuilder<TCommand, TNode, TRoot, TProcessEntity> ResolveCommandBuilder<TCommand, TNode>()
            where TCommand : ICommand<TNode, TRoot, TProcessEntity>
        {
            return _container.Resolve<ICommandBuilder<TCommand, TNode, TRoot, TProcessEntity>>();
        }

        public ICodeGenerationEngine<TSolution, TRoot, TProcessEntity> ResolveEngine()
        {
            return _engine;
        }

        public ITargetBuilder<TNode, TRoot, TProcessEntity> ResolveTargetBuilder<TNode>()
        {
            return _container.Resolve<ITargetBuilder<TNode, TRoot, TProcessEntity>>();
        }

        protected virtual void DoAutomaticRegister(ContainerBuilder builder)
        {
            var coreAssembly = Assembly.GetExecutingAssembly();

            builder.RegisterAssemblyTypes(coreAssembly)
               .Where(t => t.Name.EndsWith("CommandBuilder"))
               .AsImplementedInterfaces();

            // register the engine as singleton
            builder.RegisterInstance(_engine).As<ICodeGenerationEngine<TSolution, TRoot, TProcessEntity>>().ExternallyOwned();
        }
    }
}
