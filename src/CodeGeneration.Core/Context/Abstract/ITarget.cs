using CodeGen.Core;
using System;
using System.Collections.Generic;
namespace CodeGen.Context
{
    public abstract partial class CodeGenContext<TProject, TBaseNode, TRootNode, TSemanticModel, TProcessEntity>
        where TRootNode : TBaseNode
    {
        public interface ITarget<TNode>:Core.ITarget
            where TNode:TBaseNode
        {
            TCommand Execute<TCommand>(Func<TCommand, TCommand> commandModifiers)
                where TCommand : ICommand<TNode>;
        }

        public interface ISingleTarget<TNode> : ITarget<TNode> 
            where TNode:TBaseNode
        {
            TNode Node { get; }
            TSemanticModel SemanticSymbol { get; }
            string DocumentPath { get; }
        }

        public interface ISingleTarget<TNode0, TNode1> : ISingleTarget<TNode0>
            where TNode0 : TBaseNode
            where TNode1 : TBaseNode
        {
            ISingleTarget<TNode1> Parent { get; }
        }

        public interface ISingleTarget<TNode0, TNode1,TNode2> : ISingleTarget<TNode0>
            where TNode0 : TBaseNode
            where TNode1 : TBaseNode
            where TNode2 : TBaseNode
        {
            ISingleTarget<TNode1,TNode2> Parent { get; }
            ISingleTarget<TNode2> Grandparent { get; }
        }

        public abstract class MultipleTargeter<MultipleTarger,SingleTarget,TNode> :ITarget<TNode>
            where MultipleTarger:ITarget<TNode>
            where SingleTarget:ISingleTarget<TNode>
            where TNode:TBaseNode
        {
            protected MultipleTargeter(ICodeGenerationEngine engine)
            {
                this.CodeGenerationEngine = engine;
                this.Where(singleTarget => true);
            }
            protected readonly ICodeGenerationEngine CodeGenerationEngine;
            protected Func<SingleTarget, bool> WhereSelector;
            
            public MultipleTarger Where(Func<SingleTarget, bool> whereSelector) 
            {
                if (WhereSelector == null)
                    WhereSelector = whereSelector;
                else
                {
                    var previous = this.WhereSelector;
                    WhereSelector = (single) => previous(single) && whereSelector(single);
                }
                return (MultipleTarger)(ITarget<TNode>)this;
            }
            //Not need to implement IEnumerable to use in a foreach statement.
            //Only need to have a public GetEnumeratorMethod().
            //Wich allows to avoid the unwanted use of linq in Targets
            public abstract IEnumerator<SingleTarget> GetEnumerator();

            public TCommand Execute<TCommand>(Func<TCommand, TCommand> commandModifiers) where TCommand : ICommand<TNode>
            {
                return CodeGenerationEngine.Execute(this, commandModifiers);
            }
        }

        public abstract class SingleTarget<TNode> : ISingleTarget<TNode>
            where TNode : TBaseNode
        {
            protected SingleTarget(ICodeGenerationEngine engine, TNode node)
            {
                this.CodeGenerationEngine = engine;
                this.Node = node;
            }
            private ICodeGenerationEngine CodeGenerationEngine { get; }
            public TNode Node { get; }

            public abstract TSemanticModel SemanticSymbol { get; }
            public abstract string DocumentPath { get; }

            public TCommand Execute<TCommand>(Func<TCommand, TCommand> commandModifiers)
                where TCommand : ICommand<TNode>
            {
                return CodeGenerationEngine.Execute(this, commandModifiers);
            }
        }

        public abstract class SingleTarget<TNode0, TNode1> : ISingleTarget<TNode0,TNode1>
            where TNode0 : TBaseNode
            where TNode1 : TBaseNode
        {
            public abstract ISingleTarget<TNode1> Parent { get; }
            protected SingleTarget(ICodeGenerationEngine engine, TNode0 node0)
            {
                this.CodeGenerationEngine = engine;
                this.Node = node0;
            }
            private ICodeGenerationEngine CodeGenerationEngine { get; }
            public TNode0 Node { get; }
            public abstract TSemanticModel SemanticSymbol { get; }
            public abstract string DocumentPath { get; }

            public TCommand Execute<TCommand>(Func<TCommand, TCommand> commandModifiers)
                where TCommand : ICommand<TNode0>
            {
                return CodeGenerationEngine.Execute(this, commandModifiers);
            }
        }

        public abstract class SingleTarget<TNode0, TNode1, TNode2> : ISingleTarget<TNode0, TNode1, TNode2>
            where TNode0 : TBaseNode
            where TNode1 : TBaseNode
            where TNode2 : TBaseNode
        {
            public abstract ISingleTarget<TNode1,TNode2> Parent { get; }
            public ISingleTarget<TNode2> Grandparent => Parent.Parent;
            private ICodeGenerationEngine CodeGenerationEngine { get; }
            public TNode0 Node { get; }
            public abstract TSemanticModel SemanticSymbol { get; }
            public abstract string DocumentPath { get; }

            protected SingleTarget(ICodeGenerationEngine engine, TNode0 node0)
            {
                this.CodeGenerationEngine = engine;
                this.Node = node0;
            }

            public TCommand Execute<TCommand>(Func<TCommand, TCommand> commandModifiers)
                where TCommand : ICommand<TNode0>
            {
                return CodeGenerationEngine.Execute(this, commandModifiers);
            }
        }

    }
}
