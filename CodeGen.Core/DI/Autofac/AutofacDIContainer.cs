using Microsoft.CodeAnalysis.Editing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Autofac;
using CodeGen.DI.Abstract;

namespace CodeGen.DI
{
    public class AutofacResolver<TSolution, TRoot, TProcessEntity> : ICodeGenerationResolver<TSolution, TRoot, TProcessEntity>
    {
        protected IContainer _container { get; set; }
        protected Context.CodeGenContext<TSolution, TRoot, TProcessEntity>.ICodeGenerationEngine _engine;

        public virtual void BuildContainer()
        {
            var builder = new ContainerBuilder();

            _container = builder.Build();
        }

        public void RegisterEngine(Context.CodeGenContext<TSolution, TRoot, TProcessEntity>.ICodeGenerationEngine engine)
        {
            _engine = engine;
        }

        public Context.CodeGenContext<TSolution,TRoot,TProcessEntity>.ICommandBuilder<TCommand, TNode> ResolveCommandBuilder<TCommand, TNode>()
            where TCommand : Context.CodeGenContext<TSolution, TRoot, TProcessEntity>.ICommand<TNode>
        {
            return _container.Resolve< Context.CodeGenContext < TSolution,TRoot,TProcessEntity>.ICommandBuilder<TCommand,TNode>>();
        }

        public Context.CodeGenContext<TSolution, TRoot, TProcessEntity>.ICodeGenerationEngine ResolveEngine()
        {
            return _engine;
        }

        public Context.CodeGenContext<TSolution, TRoot, TProcessEntity>.ITargetBuilder<TNode> ResolveTargetBuilder<TNode>()
        {
            return _container.Resolve< Context.CodeGenContext <TSolution,TRoot,TProcessEntity>.ITargetBuilder <TNode>>();
        }

        protected virtual void DoAutomaticRegister(ContainerBuilder builder)
        {
            var coreAssembly = Assembly.GetExecutingAssembly();

            builder.RegisterAssemblyTypes(coreAssembly)
               .Where(t => t.Name.EndsWith("CommandBuilder"))
               .AsImplementedInterfaces();

            // register the engine as singleton
            builder.RegisterInstance(_engine).As<Context.CodeGenContext<TSolution,TRoot,TProcessEntity>.ICodeGenerationEngine>().ExternallyOwned();
        }
    }
}
