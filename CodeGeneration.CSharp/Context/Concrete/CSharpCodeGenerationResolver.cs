using Autofac;
using CodeGen.Attributes;
using CodeGen.Context;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using System.Linq;
using System.Reflection;

namespace CodeGen.CSharp.Context
{
    public abstract partial class CSharpContext<TProcessEntity> : CodeGenContext<Project, CSharpSyntaxNode, ISymbol,TProcessEntity>
    {
        public class CSharpAutofacResolver : ICodeGenerationResolver
        {
            protected IContainer _container { get; set; }
            protected ICodeGenerationEngine _engine;
            public CSharpAutofacResolver()
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
                where TNode : CSharpSyntaxNode
            {
                return _container.Resolve<ChainTargetBuilder<TNode>>();
            }

            public ITarget<TSyntaxNode> ResolveTarget<TSyntaxNode>()
                where TSyntaxNode:CSharpSyntaxNode
            {
                return _container.Resolve<ITarget<TSyntaxNode>>();
            }

            public TCommandBuilder ResolveCommandBuilder<TCommandBuilder, TSyntaxNode>()
                where TCommandBuilder : ICommandBuilder<TSyntaxNode>
                where TSyntaxNode : CSharpSyntaxNode
            {
                return _container.Resolve<TCommandBuilder>();
            }

            public ICommandHandler<TSyntaxNode> ResolveCommandHandler<TSyntaxNode>(ICommandBuilder<TSyntaxNode> commandBuilder)
                where TSyntaxNode : CSharpSyntaxNode
            {
                var cmdbuildertype = commandBuilder.GetType().GetInterfaces().First();
                var syntaxtype = typeof(TSyntaxNode);
                var handlertype = typeof(ICommandHandler<,>).MakeGenericType(new[] { typeof(Project), typeof(CSharpSyntaxNode),typeof(ISymbol), typeof(TProcessEntity), cmdbuildertype, syntaxtype });
                return (ICommandHandler<TSyntaxNode>)_container.Resolve(handlertype, new[] { new PositionalParameter(0, commandBuilder) });
            }
            protected void DoAutomaticRegister(ContainerBuilder builder)
            {
                builder.RegisterGeneric(typeof(CSharpTarget<>)).As(typeof(ICSharpTarget<>));
                builder.RegisterGeneric(typeof(CSharpTarget<>)).As(typeof(ITarget<>));
                builder.RegisterGeneric(typeof(CSharpTargetBuilder<>)).As(typeof(TargetBuilder<>));
                builder.RegisterGeneric(typeof(ChainCSharpTargetBuilder<>)).As(typeof(ChainTargetBuilder<>));

                var coreAssembly = Assembly.GetExecutingAssembly();

                ////Register Command Builders only as generic services
                foreach (var t in coreAssembly.GetTypes().Where(x => x.CustomAttributes.Any(a => a.AttributeType == typeof(CommandBuilderAttribute))))
                    builder.RegisterGeneric(t).As(t.GetInterfaces().First());//TODO: fix here only explicit interfaces

                //Register Command Handlers only as generic services
                foreach (var t in coreAssembly.GetTypes().Where(x => x.CustomAttributes.Any(a => a.AttributeType == typeof(CommandHandlerAttribute))))
                    builder.RegisterType(t).As(t.GetInterfaces().First());//TODO: fix here only explicit interfaces

                // register the engine as singleton
                builder.RegisterInstance(_engine).As<ICodeGenerationEngine>().ExternallyOwned();
            }

        }
    }
}