using CodeGen.Core;
using System;
using System.Collections.Generic;
namespace CodeGen.Context
{
    public abstract partial class CodeGenContext<TProject, TBaseNode,TRootNode,TSemanticModel,TProcessEntity> 
        where TRootNode:TBaseNode
    {
        public interface ITarget<TNode> : ITarget 
            where TNode:TBaseNode
        {
            Func<TSemanticModel, TNode, bool> WhereSelector { get; set; }
            ICodeGenerationEngine CodeGenerationEngine { get; set; }

            public ITarget<TNode> Where(Func<TSemanticModel, TNode, bool> filter)
            {
                this.WhereSelector = filter;
                return this;
            }

            public ITarget<TNode> Where(Func<TNode, bool> filter)
            {
                this.WhereSelector = (_, node) => filter(node);
                return this;
            }

            TCommand Execute<TCommand>()
                where TCommand : ICommand<TNode>;
            
        }

        public abstract class Target<TNode> : ITarget<TNode>
            where TNode:TBaseNode
        {
            public Func<TSemanticModel, TNode, bool> WhereSelector { get ; set ; }
            public ICodeGenerationEngine CodeGenerationEngine { get; set; }

            protected Target(ICodeGenerationEngine engine)
            {
                this.CodeGenerationEngine = engine;
                (this as ITarget<TNode>).Where((semantic,node) =>true);
            }

            public abstract IEnumerable<TNode> Select(TBaseNode root, Func<TNode, TSemanticModel> semanticModelSelector);

            public TCommand Execute<TCommand>() where TCommand : ICommand<TNode>
            {
                return CodeGenerationEngine.Execute<TCommand, TNode>(this);
            }
        }
    }
}
