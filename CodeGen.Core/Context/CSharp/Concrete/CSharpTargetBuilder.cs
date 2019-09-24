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

            public ITarget<TNode> Build()
            {
                return new CSharpTarget<TNode>(((ITargetBuilder)this).WhereSelector);
            }

            public ICommandBuilder<TCommand> Execute<TCommand>() where TCommand : ICommand<TNode>
            {
                throw new NotImplementedException();
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

            ITarget<CSharpSyntaxNode> ITargetBuilder<TNode>.Build()
            {
                throw new NotImplementedException();
            }

            ICommandBuilder<TCommand> ITargetBuilder.Execute<TCommand>()
            {
                throw new NotImplementedException();
            }

            ITargetBuilder ITargetBuilder.Where(Func<object, bool> filter)
            {
                WhereSelector = filter;
                return this;
            }

            ITargetBuilder<TNode> ITargetBuilder<TNode>.Where(Func<TNode, bool> filter)
            {
                WhereSelector = filter;
                return this;
            }
        }
    }
}