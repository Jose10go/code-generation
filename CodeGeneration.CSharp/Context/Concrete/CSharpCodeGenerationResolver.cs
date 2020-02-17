using Autofac;
using CodeGen.Attributes;
using CodeGen.Context;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using System.Linq;
using System.Reflection;
using static CodeGen.CSharp.Context.DocumentEdit.CSharpContextDocumentEditor;

namespace CodeGen.CSharp.Context
{
    public abstract partial class CSharpContext<TProcessEntity> : CodeGenContext<Project, CSharpSyntaxNode, ISymbol,TProcessEntity>
    {
        public class CSharpAutofacResolver : ICodeGenerationResolver
        {
            protected IContainer _container { get; set; }
            private ContainerBuilder builder;
            
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

            public TCommandBuilder ResolveCommandBuilder<TCommandBuilder, TSyntaxNode>()
                where TCommandBuilder : ICommand<TSyntaxNode>
                where TSyntaxNode : CSharpSyntaxNode
            {
                return _container.Resolve<TCommandBuilder>();
            }

            public ICommandHandler ResolveCommandHandler(Core.ICommand commandBuilder)
            {
                var cmdtype = commandBuilder.GetType();
                var handlertype = typeof(ICommandHandler<>).MakeGenericType(new[] { typeof(Project), typeof(CSharpSyntaxNode),typeof(ISymbol), typeof(TProcessEntity), cmdtype });
                return (ICommandHandler)_container.Resolve(handlertype, new[] { new PositionalParameter(0, commandBuilder) });
            }
            
            protected void DoAutomaticRegister(ContainerBuilder builder)
            {
                var coreAssembly = Assembly.GetExecutingAssembly();

                ////Register Command Builders only as generic services
                //foreach (var t in coreAssembly.GetTypes().Where(x => x.CustomAttributes.Any(a => a.AttributeType == typeof(CommandBuilderAttribute))))
                //    builder.RegisterType(t).As(t.GetInterfaces().First());//TODO: fix here only explicit interfaces
                builder.RegisterType<ClassCloneCommand>().As<IClassClone>();

                //Register Command Handlers only as generic services
                foreach (var t in coreAssembly.GetTypes().Where(x => x.CustomAttributes.Any(a => a.AttributeType == typeof(CommandHandlerAttribute))))
                    builder.RegisterType(t).As(t.GetInterfaces().First());//TODO: fix here only explicit interfaces

            }

        }
    }
}