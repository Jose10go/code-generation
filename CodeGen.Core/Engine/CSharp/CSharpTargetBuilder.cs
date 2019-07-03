using CodeGen.Commands;
using CodeGen.Commands.Abstract;
using CodeGen.DI.Abstract;
using CodeGen.Engine.Abstract;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeGen.Engine.CSharp.DocumentEditing
{
    public class CSharpTargetBuilder<TNode, TProcessEntity> : ICSharpTargetBuilder<TNode, TProcessEntity>
        where TNode: CSharpSyntaxNode
    {
        ICodeGenerationResolver<Solution, CSharpSyntaxNode, TProcessEntity> _resolver;

        public CSharpTargetBuilder(ICodeGenerationResolver<Solution, CSharpSyntaxNode, TProcessEntity> resolver)
        {
            _resolver = resolver;
        }

        public Func<TNode, bool> WhereSelector { get; set; }

        Func<object, bool> ITargetBuilder.WhereSelector => obj => WhereSelector((TNode)obj);

        public ITarget<TNode, CSharpSyntaxNode> Build()
        {
            return new CSharpTarget<TNode>(((ITargetBuilder)this).WhereSelector);
        }

        public ICommandBuilder<TCommand> Execute<TCommand>() where TCommand : ICommand<TNode, CSharpSyntaxNode, TProcessEntity>
        {
            throw new NotImplementedException();
        }

        public ITargetBuilder<TNode, CSharpSyntaxNode, TProcessEntity> Where(Func<TNode, bool> filter)
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
            throw new NotImplementedException();
        }

        ITargetBuilder ITargetBuilder.Where(Func<object, bool> filter)
        {
            WhereSelector = filter;

            return this;
        }
    }
}
