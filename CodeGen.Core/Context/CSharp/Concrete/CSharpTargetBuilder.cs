using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using System;
using static CodeGen.Context.CodeGenTypelessContext;

namespace CodeGen.Context.CSharp
{
    public partial class CSharpContext<TProcessEntity> : CodeGenContext<Solution, CSharpSyntaxNode, TProcessEntity>
    {
        public class CSharpTargetBuilder<TNode> : ICSharpTargetBuilder<TNode>
            where TNode : CSharpSyntaxNode
        {
            private Func<TNode, bool> WhereSelector { get; set; }

            public ITarget<CSharpSyntaxNode> Build()
            {
                return new CSharpTarget<TNode>(this.WhereSelector);
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

        }

        public class ChainCSharpTargetBuilder<TNode> : IChainCSharpTargetBuilder<TNode>,IChainTargetBuilderSelector<TNode>
           where TNode : CSharpSyntaxNode
        {
            private Func<TNode, bool> WhereSelector { get; set; }

            public TCommandBuilder Execute<TCommand, TCommandBuilder>()
                where TCommand : ICommand<TNode>
                where TCommandBuilder : ICommandBuilder<TCommand>
            {
                var commandbuilder = (TCommandBuilder)Resolver.ResolveCommandBuilder<TCommand, TNode>();
                commandbuilder.Target =new CSharpTarget<TNode>(this.WhereSelector);
                return commandbuilder;
            }

            public IChainTargetBuilder<TNode> Where(Func<TNode, bool> filter)
            {
                WhereSelector = filter;
                return this;
            }
        }
    }

}