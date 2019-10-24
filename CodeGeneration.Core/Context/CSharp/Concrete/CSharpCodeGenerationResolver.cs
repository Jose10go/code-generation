using Autofac;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;

namespace CodeGen.Context.CSharp
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

                base.DoAutomaticRegister(builder);
            }

        }
    }
}