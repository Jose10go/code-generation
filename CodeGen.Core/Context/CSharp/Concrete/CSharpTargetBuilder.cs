using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using System;
namespace CodeGen.Context.CSharp
{
    public partial class ICSharpContext<TProcessEntity> : CodeGenContext<Solution, CSharpSyntaxNode, TProcessEntity>
    {
        public class CSharpTargetBuilder<TNode> : ICSharpTargetBuilder<TNode>
            where TNode : CSharpSyntaxNode
        {
            ICodeGenerationResolver _resolver;

            public CSharpTargetBuilder(ICodeGenerationResolver resolver)
            {
                _resolver = resolver;
            }
            private Func<TNode, bool> WhereSelector { get; set; }

            public ITarget<CSharpSyntaxNode> Build()
            {
                return new CSharpTarget<TNode>(this.WhereSelector);
            }

            public ICommandBuilder<TCommand> Execute<TCommand>() where TCommand : ICommand<TNode>
            {
                 return _resolver.ResolveCommandBuilder<TCommand,TNode>();
            }

            public ITargetBuilder<TNode> Where(Func<TNode, bool> filter)
            {
                WhereSelector = filter;
                return this;
            }

            ITarget ITargetBuilder.Build()
            {
                return Build();
            }
            ICommandBuilder<TCommand> ITargetBuilder.Execute<TCommand>()
            {
                return (ICommandBuilder<TCommand>)Execute<ICommand<TNode>>();
            }

            ITargetBuilder ITargetBuilder.Where(Func<object, bool> filter)
            {
                return Where(filter);
            }

        }
    }
}