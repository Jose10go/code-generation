using System;
using System.Collections.Generic;

namespace CodeGen.Context
{
    public abstract partial class CodeGenContext<TProject, TBaseNode, TRootNode, TSemanticModel, TProcessEntity>
        where TRootNode : TBaseNode
    {
        public interface ITarget<TNode> : Core.ITarget
            where TNode : TBaseNode
        {
            ITarget<TOutNode> Execute<TCommand, TOutNode>(Func<TCommand,TOutNode> commandModifiers)
                where TCommand : ICommand<TNode, TOutNode>
                where TOutNode : TBaseNode;
        }

        public abstract class MultipleTargeter<MultipleTarger, SingleTarget, TNode> : ITarget<TNode>
            where MultipleTarger : ITarget<TNode>
            where SingleTarget : SingleTarget<TNode>
            where TNode : TBaseNode
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
            //Wich allows to avoid the unwanted use of linq in Targets.
            public abstract IEnumerator<SingleTarget> GetEnumerator();

            ITarget<TOutput> ITarget<TNode>.Execute<TCommand, TOutput>(Func<TCommand, TOutput> commandModifiers)
            {
                return this.Execute(commandModifiers);
            }

            public MultipleTarget<TOutput> Execute<TCommand,TOutput>(Func<TCommand,TOutput> commandModifiers)
                where TCommand : ICommand<TNode, TOutput>
                where TOutput : TBaseNode
            {
                var command = this.CodeGenerationEngine.CodeGenerationResolver.ResolveCommandBuilder<TCommand, TNode, TOutput>();
                var handler = this.CodeGenerationEngine.CodeGenerationResolver.ResolveCommandHandler<TCommand, TNode, TOutput>(command);
                commandModifiers(command);
                Dictionary<string, TProcessEntity> memoized = new Dictionary<string, TProcessEntity>();
                foreach (var singleTarget in this)
                {
                    TProcessEntity documentEditor;
                    if (!memoized.ContainsKey(singleTarget.DocumentPath))
                        memoized.Add(singleTarget.DocumentPath,this.CodeGenerationEngine.GetProccesEntity(singleTarget));
                    documentEditor = memoized[singleTarget.DocumentPath];
                    handler.ProccessNode(singleTarget, documentEditor,CodeGenerationEngine);
                }

                foreach (var item in memoized)
                    this.CodeGenerationEngine.UpdateProject(item.Value);
                
                return CodeGenerationEngine.Select<TOutput>();
            }

        }

        public abstract class SingleTarget<TNode>:ITarget<TNode>
            where TNode : TBaseNode
        {
            protected SingleTarget(ICodeGenerationEngine engine, TNode node)
            {
                this.CodeGenerationEngine = engine;
                this.Node = node;
            }
            public TNode Node { get; }
            protected ICodeGenerationEngine CodeGenerationEngine { get; }

            public abstract TSemanticModel SemanticSymbol { get; }
            public abstract string DocumentPath { get; }

            ITarget<TOutput> ITarget<TNode>.Execute<TCommand, TOutput>(Func<TCommand, TOutput> commandModifiers)
            {
                return this.Execute(commandModifiers);
            }

            public SingleTarget<TOutput> Execute<TCommand,TOutput>(Func<TCommand,TOutput> commandModifiers)
                where TCommand : ICommand<TNode,TOutput>
                where TOutput:TBaseNode
            {
                var command = this.CodeGenerationEngine.CodeGenerationResolver.ResolveCommandBuilder<TCommand,TNode,TOutput>();
                commandModifiers(command);
                var handler = this.CodeGenerationEngine.CodeGenerationResolver.ResolveCommandHandler<TCommand,TNode,TOutput>(command);

                var processEntity = this.CodeGenerationEngine.GetProccesEntity(this);
                var outputTarget=handler.ProccessNode(this, processEntity,this.CodeGenerationEngine);
                this.CodeGenerationEngine.UpdateProject(processEntity);
                
                return outputTarget;
            }
        }

        public abstract class SingleTarget<TNode0, TNode1> : SingleTarget<TNode0>
            where TNode0 : TBaseNode
            where TNode1 : TBaseNode
        {
            public abstract SingleTarget<TNode1> Parent { get; }
            protected SingleTarget(ICodeGenerationEngine engine, TNode0 node0):base(engine,node0)
            {
            }
        }

        public abstract class SingleTarget<TNode0, TNode1, TNode2> : SingleTarget<TNode0>
            where TNode0 : TBaseNode
            where TNode1 : TBaseNode
            where TNode2 : TBaseNode
        {
            public abstract SingleTarget<TNode1,TNode2> Parent { get; }
            public SingleTarget<TNode2> Grandparent => Parent.Parent;

            protected SingleTarget(ICodeGenerationEngine engine, TNode0 node0):base(engine,node0)
            {
            }
        }

        public abstract class MultipleTarget<TNode> : MultipleTargeter<MultipleTarget<TNode>, SingleTarget<TNode>, TNode> 
            where TNode:TBaseNode
        {
            protected MultipleTarget(ICodeGenerationEngine codeGenerationEngine):base(codeGenerationEngine)
            {
            }
        }

        public abstract class MultipleTarget<TNode0,TNode1> : MultipleTargeter<MultipleTarget<TNode0, TNode1>, SingleTarget<TNode0, TNode1>, TNode0>
            where TNode0 : TBaseNode
            where TNode1 : TBaseNode
        {
            protected MultipleTarget(ICodeGenerationEngine codeGenerationEngine) : base(codeGenerationEngine)
            {
            }
        }

        public abstract class MultipleTarget<TNode0, TNode1, TNode2> : MultipleTargeter<MultipleTarget<TNode0, TNode1, TNode2>, SingleTarget<TNode0, TNode1, TNode2>, TNode0>
            where TNode0 : TBaseNode
            where TNode1 : TBaseNode
            where TNode2 : TBaseNode
        {
            protected MultipleTarget(ICodeGenerationEngine codeGenerationEngine) : base(codeGenerationEngine)
            {
            }
        }
    }
}
