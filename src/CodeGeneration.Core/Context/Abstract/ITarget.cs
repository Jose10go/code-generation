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
            ICodeGenerationEngine CodeGenerationEngine { get;}

            TCommand Execute<TCommand>(Func<TCommand,TCommand> commandModifiers)
                where TCommand : ICommand<TNode>;
            
        }

        public interface ISingleTarget<TNode> : ITarget<TNode> 
            where TNode:TBaseNode
        {
            public TNode Node { get; }
            public TSemanticModel SemanticSymbol { get; }
            public string DocumentPath { get; }
        }

        public abstract class SingleTarget<TNode> : ISingleTarget<TNode>
            where TNode:TBaseNode
        {
            protected SingleTarget(ICodeGenerationEngine engine,TNode node)
            {
                this.CodeGenerationEngine = engine;
                this.Node = node;
            }
            public ICodeGenerationEngine CodeGenerationEngine { get;}
            public TNode Node { get; }
            public abstract TSemanticModel SemanticSymbol { get; }
            public abstract string DocumentPath { get; }

            public TCommand Execute<TCommand>(Func<TCommand, TCommand> commandModifiers)
                where TCommand : ICommand<TNode>
            {
                return CodeGenerationEngine.Execute(this,commandModifiers);
            }
        }

        public abstract class Target<TNode> : ITarget<TNode>
            where TNode:TBaseNode
        {
            public Func<SingleTarget<TNode>, bool> WhereSelector { get ; set ; }
            public ICodeGenerationEngine CodeGenerationEngine { get; set; }

            protected Target(ICodeGenerationEngine engine)
            {
                this.CodeGenerationEngine = engine;
                this.Where(singleTarget =>true);
            }

            public TCommand Execute<TCommand>(Func<TCommand, TCommand> commandModifiers)
                where TCommand : ICommand<TNode>
            {
                return CodeGenerationEngine.Execute(this,commandModifiers);
            }

            public Target<TNode> Where(Func<SingleTarget<TNode>,bool> filter)
            {
                if (WhereSelector == null)
                    WhereSelector = filter;
                else
                {
                    var previous = this.WhereSelector;
                    WhereSelector = (single) => previous(single) && filter(single);
                }
                return this;
            }

            //Not need to implement IEnumerable to use in a foreach statement.
            //Only need to have a public GetEnumeratorMethod().
            //Wich allows to avoid the unwanted use of linq in Targets
            public abstract IEnumerator<ISingleTarget<TNode>> GetEnumerator();
        }
    }
}
