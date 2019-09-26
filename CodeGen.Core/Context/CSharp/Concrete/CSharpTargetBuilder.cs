using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using System;
using static CodeGen.Context.CodeGenTypelessContext;

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

            public TCommandBuilder Execute<TCommand, TCommandBuilder>()
                 where TCommand : ICommand<TNode>
                 where TCommandBuilder : ICommandBuilder<TCommand>
            {
                 return (TCommandBuilder)_resolver.ResolveCommandBuilder<TCommand,TNode>();
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

            ITargetBuilder ITargetBuilder.Where(Func<object, bool> filter)
            {
                return Where(filter);
            }

            TCommandBuilder ITargetBuilder.Execute<TCommand, TCommandBuilder>()
            {
                return (TCommandBuilder)Execute<ICommand<TNode>,ICommandBuilder<ICommand<TNode>>>();
            }
        }
    }
}