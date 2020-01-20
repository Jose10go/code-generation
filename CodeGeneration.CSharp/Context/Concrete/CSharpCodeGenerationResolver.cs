using Autofac;
using CodeGen.Attributes;
using CodeGen.Context;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using System.Linq;
using System.Reflection;

namespace CodeGen.CSharp.Context
{
    public partial class CSharpContext<TProcessEntity> : CodeGenContext<Solution, CSharpSyntaxNode, TProcessEntity>
    {
        public class CSharpAutofacResolver : AutofacResolver
        {
            protected override void DoAutomaticRegister(ContainerBuilder builder)
            {
                builder.RegisterGeneric(typeof(CSharpTarget<>)).As(typeof(ICSharpTarget<>));
                builder.RegisterGeneric(typeof(CSharpTarget<>)).As(typeof(ITarget<>));
                builder.RegisterGeneric(typeof(CSharpTargetBuilder<>)).As(typeof(ICSharpTargetBuilder<>));
                builder.RegisterGeneric(typeof(CSharpTargetBuilder<>)).As(typeof(ITargetBuilder<>));
                builder.RegisterGeneric(typeof(ChainCSharpTargetBuilder<>)).As(typeof(IChainTargetBuilder<>));

                var coreAssembly = Assembly.GetExecutingAssembly();

                ////Register Command Builders only as generic services
                //foreach (var t in coreAssembly.GetTypes().Where(x => x.CustomAttributes.Any(a => a.AttributeType == typeof(CommandBuilderAttribute))))
                //    foreach (var i in t.GetInterfaces().Where(x => x.IsGenericType))
                //        builder.RegisterGeneric(t).As(i);
                builder.RegisterType(typeof(MethodCloneCommandBuilder)).As<IMethodClone>();

                //Register Command Handlers only as generic services
                foreach (var t in coreAssembly.GetTypes().Where(x => x.CustomAttributes.Any(a => a.AttributeType == typeof(CommandHandlerAttribute))))
                    //foreach (var i in t.GetInterfaces().Where(x => x.IsGenericType))
                        builder.RegisterType(t).AsSelf();

                // register the engine as singleton
                builder.RegisterInstance(_engine).As<ICodeGenerationEngine>().ExternallyOwned();
            }

        }
    }
}