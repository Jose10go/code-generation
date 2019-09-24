using Microsoft.CodeAnalysis.Editing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Autofac;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;

namespace CodeGen.Context
{
    public partial class CodeGenContext<TProject, TRootNode, TProcessEntity>:CodeGenTypelessContext
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

                builder.RegisterAssemblyTypes(coreAssembly)
                   .Where(t => t.Name.EndsWith("CommandBuilder"))
                   .AsImplementedInterfaces();

                // register the engine as singleton
                builder.RegisterInstance(_engine).As<ICodeGenerationEngine>().ExternallyOwned();
            }
        }
    }
}
