using CodeGen.Commands;
using CodeGen.DI.Abstract;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace CodeGen.Context.CSharp
{
    public partial class ICSharpContext<TProcessEntity> : CodeGen.Context.CodeGenContext<Solution, CSharpSyntaxNode, TProcessEntity>
    {
        public class CSharpTargetBuilder<TNode> : ICSharpTargetBuilder<TNode>
            where TNode : CSharpSyntaxNode
        {
            ICodeGenerationResolver<Solution, CSharpSyntaxNode, TProcessEntity> _resolver;

            public CSharpTargetBuilder(ICodeGenerationResolver<Solution, CSharpSyntaxNode, TProcessEntity> resolver)
            {
                _resolver = resolver;
            }

            public Func<TNode, bool> WhereSelector { get; set; }

            Func<object, bool> ITargetBuilder.WhereSelector => obj => WhereSelector((TNode)obj);

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