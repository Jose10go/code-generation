using System;
using System.Collections.Generic;
using System.Linq;

namespace CodeGen.Context
{
    public abstract partial class CodeGenContext<TProject, TBaseNode, TRootNode, TSemanticModel, TProcessEntity>
        where TRootNode : TBaseNode
    {
        public sealed class Key<T>
            where T:class
        {
            private readonly int id;
            internal Key(int id)
            {
                this.id = id;
            }

            public override int GetHashCode()
            {
                return id;
            }

            public override bool Equals(object obj)
            {
                if (!(obj is Key<T>))
                    return false;
                var other = obj as Key<T>;
                return id==other.id;
            }

            public override string ToString()
            {
                return $"Key<{typeof(T)}>-{id}";
            }
        }

        public interface ITarget<TNode> : Core.ITarget
            where TNode : TBaseNode
        {
            ITarget<TOutNode> Execute<TCommand, TOutNode>(Func<TCommand,TOutNode> commandModifiers)
                where TCommand : ICommand<TNode, TOutNode>
                where TOutNode : TBaseNode;

        }

        public interface ISingleTarget<TNode>:ITarget<TNode> 
            where TNode:TBaseNode
        {
            public TNode Node { get; }
            public abstract TSemanticModel SemanticSymbol { get; }
            public abstract string DocumentPath { get; }
            new SingleTarget<TOutNode> Execute<TCommand, TOutNode>(Func<TCommand, TOutNode> commandModifiers)
                where TCommand : ICommand<TNode, TOutNode>
                where TOutNode : TBaseNode;
        }

        public abstract class MultipleTargeter<MultipleTarget, SingleTarget, TNode> : ITarget<TNode>
            where MultipleTarget : ITarget<TNode>
            where SingleTarget : ISingleTarget<TNode>
            where TNode : TBaseNode
        {
            protected MultipleTargeter(ICodeGenerationEngine engine, IEnumerable<SingleTarget> singleTargets)
            {
                this.CodeGenerationEngine = engine;
                this.Where(singleTarget => true);
                this.singleTargets = singleTargets;
                this.UsingSelectors = new List<Func<SingleTarget, object>>();
            }
            
            private readonly IEnumerable<SingleTarget> singleTargets;
            protected readonly ICodeGenerationEngine CodeGenerationEngine;
            protected Func<SingleTarget, bool> WhereSelector;
            protected List<Func<SingleTarget,object>> UsingSelectors;
            public MultipleTarget Where(Func<SingleTarget, bool> whereSelector)
            {
                if (WhereSelector == null)
                    WhereSelector = whereSelector;
                else
                {
                    var previous = this.WhereSelector;
                    WhereSelector = (single) => previous(single) && whereSelector(single);
                }
                return (MultipleTarget)(ITarget<TNode>)this;
            }
            
            //Not need to implement IEnumerable to use in a foreach statement.
            //Only need to have a public GetEnumeratorMethod().
            //Wich allows to avoid the unwanted use of linq in Targets.
            public IEnumerator<SingleTarget> GetEnumerator()
            {
                if (this.singleTargets != null)
                    return singleTargets.GetEnumerator();

                return this.CodeGenerationEngine.GetRootNodes()
                                                .SelectMany(root => SelectedNodes(root))
                                                .GetEnumerator();
            }

            internal abstract IEnumerable<SingleTarget> SelectedNodes(TBaseNode root);

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
                IEnumerable<TOutput> outputNodes = Enumerable.Empty<TOutput>();
                foreach (var singleTarget in this)
                {
                    TProcessEntity documentEditor;
                    if (!memoized.ContainsKey(singleTarget.DocumentPath))
                        memoized.Add(singleTarget.DocumentPath, this.CodeGenerationEngine.GetProccesEntity(singleTarget));
                    documentEditor = memoized[singleTarget.DocumentPath];
                    outputNodes=outputNodes.Append(handler.ProccessNode(singleTarget.Node, documentEditor, CodeGenerationEngine));
                }

                foreach (var item in memoized)
                    this.CodeGenerationEngine.UpdateProject(item.Value);

                return new MultipleTarget<TOutput>(this.CodeGenerationEngine,outputNodes.Select(node=>new SingleTarget<TOutput>(this.CodeGenerationEngine,node)));
            }

            public MultipleTarget Using<T>(Func<SingleTarget, T> usingSelector, out Key<T> key)
                where T:class
            {
                this.UsingSelectors.Add(usingSelector);
                key = new Key<T>(usingSelector.GetHashCode());//TODO: another way to generate a key
                return (MultipleTarget)(ITarget<TNode>)this;
            }
        }

        public abstract class SingleTargeter<TSingleTarget,TNode>:ISingleTarget<TNode>
            where TSingleTarget:ITarget<TNode>
            where TNode:TBaseNode
        {
            public TNode Node { get; }
            public TSemanticModel SemanticSymbol { get; }
            public string DocumentPath { get; }
            protected ICodeGenerationEngine CodeGenerationEngine { get; }
            private readonly Dictionary<object, Func<TSingleTarget, object>> dictionary;
            
            protected SingleTargeter(ICodeGenerationEngine engine, TNode node)
            {
                this.CodeGenerationEngine = engine;
                this.Node = node;
                this.DocumentPath = engine.GetFilePath(node);
                this.SemanticSymbol = engine.GetSemantic(node);
                this.dictionary = new Dictionary<object, Func<TSingleTarget, object>>();
            }

            public TSingleTarget Get<T>(Key<T> key, out T value)
                where T : class
            {
                TSingleTarget self = (TSingleTarget)(ITarget<TNode>)this;
                value = (T)dictionary[key](self);
                return self;
            }

            public TSingleTarget Using<T>(Func<TSingleTarget,T> usingSelector, out Key<T> key)
                where T : class
            {
                TSingleTarget self = (TSingleTarget)(ITarget<TNode>)this;
                key = new Key<T>(usingSelector.GetHashCode());//TODO: another way to generate a key
                this.dictionary.Add(key, usingSelector);
                return self;
            }

            public SingleTarget<TOutput> Execute<TCommand, TOutput>(Func<TCommand, TOutput> commandModifiers)
                where TCommand : ICommand<TNode, TOutput>
                where TOutput : TBaseNode
            {
                var command = this.CodeGenerationEngine.CodeGenerationResolver.ResolveCommandBuilder<TCommand, TNode, TOutput>();
                commandModifiers(command);
                var handler = this.CodeGenerationEngine.CodeGenerationResolver.ResolveCommandHandler<TCommand, TNode, TOutput>(command);

                var processEntity = this.CodeGenerationEngine.GetProccesEntity(this);
                var outputNode = handler.ProccessNode(this.Node, processEntity, this.CodeGenerationEngine);
                this.CodeGenerationEngine.UpdateProject(processEntity);

                return new SingleTarget<TOutput>(this.CodeGenerationEngine,outputNode);
            }

            ITarget<TOutNode> ITarget<TNode>.Execute<TCommand, TOutNode>(Func<TCommand, TOutNode> commandModifiers)
            {
                return this.Execute(commandModifiers);
            }
        }

        public sealed class SingleTarget<TNode>:SingleTargeter<SingleTarget<TNode>,TNode>
            where TNode : TBaseNode
        {
            public SingleTarget(ICodeGenerationEngine engine, TNode node):base(engine,node)
            {
            }
        }

        public sealed class SingleTarget<TNode0, TNode1> : SingleTargeter<SingleTarget<TNode0, TNode1>,TNode0>
            where TNode0 : TBaseNode
            where TNode1 : TBaseNode
        {
            public SingleTarget<TNode1> Parent { get; }
            public SingleTarget(ICodeGenerationEngine engine, TNode0 node0,TNode1 node1):base(engine,node0)
            {
                Parent = new SingleTarget<TNode1>(engine, node1);
            }
        }

        public sealed class SingleTarget<TNode0, TNode1, TNode2> : SingleTargeter<SingleTarget<TNode0,TNode1,TNode2>,TNode0>
            where TNode0 : TBaseNode
            where TNode1 : TBaseNode
            where TNode2 : TBaseNode
        {
            public SingleTarget<TNode1,TNode2> Parent { get; }
            public SingleTarget<TNode2> Grandparent => Parent.Parent;

            public SingleTarget(ICodeGenerationEngine engine, TNode0 node0, TNode1 node1, TNode2 node2) :base(engine,node0)
            {
                Parent = new SingleTarget<TNode1, TNode2>(engine, node1, node2);
            }
        }

        public sealed class MultipleTarget<TNode> : MultipleTargeter<MultipleTarget<TNode>, SingleTarget<TNode>, TNode> 
            where TNode:TBaseNode
        {
            public MultipleTarget(ICodeGenerationEngine codeGenerationEngine,IEnumerable<SingleTarget<TNode>> singleTargets=null)
                :base(codeGenerationEngine,singleTargets)
            {
            }

            internal override IEnumerable<SingleTarget<TNode>> SelectedNodes(TBaseNode root)
            {
                return this.CodeGenerationEngine.GetDescendantNodes(root)
                                   .OfType<TNode>()
                                   .Select(node => {
                                       var result = new SingleTarget<TNode>(this.CodeGenerationEngine, node);
                                       foreach (var usingSelector in this.UsingSelectors)
                                           result = (SingleTarget<TNode>)usingSelector(result);
                                       return result;
                                   })
                                   .Where(x => WhereSelector(x));
            }
        }

        public sealed class MultipleTarget<TNode0,TNode1> : MultipleTargeter<MultipleTarget<TNode0, TNode1>, SingleTarget<TNode0, TNode1>, TNode0>
            where TNode0 : TBaseNode
            where TNode1 : TBaseNode
        {
            public MultipleTarget(ICodeGenerationEngine codeGenerationEngine,IEnumerable<SingleTarget<TNode0,TNode1>> singleTargets=null)
                : base(codeGenerationEngine, singleTargets)
            {
            }

            internal override IEnumerable<SingleTarget<TNode0, TNode1>> SelectedNodes(TBaseNode root)
            {
                return this.CodeGenerationEngine
                           .GetDescendantNodes(root)
                           .OfType<TNode1>()
                           .SelectMany(parent => this.CodeGenerationEngine.GetDescendantNodes(parent)
                                                     .OfType<TNode0>()
                                                     .Select(node => {
                                                         var result = new SingleTarget<TNode0,TNode1>(this.CodeGenerationEngine, node, parent);
                                                         foreach (var usingSelector in this.UsingSelectors)
                                                             result = (SingleTarget<TNode0, TNode1>)usingSelector(result);
                                                         return result;
                                                     }))
                           .Where(x => WhereSelector(x));
            }
        }

        public sealed class MultipleTarget<TNode0, TNode1, TNode2> : MultipleTargeter<MultipleTarget<TNode0, TNode1, TNode2>, SingleTarget<TNode0, TNode1, TNode2>, TNode0>
            where TNode0 : TBaseNode
            where TNode1 : TBaseNode
            where TNode2 : TBaseNode
        {
            public MultipleTarget(ICodeGenerationEngine codeGenerationEngine,IEnumerable<SingleTarget<TNode0,TNode1,TNode2>> singleTargets=null)
                : base(codeGenerationEngine,singleTargets)
            {
            }

            internal override IEnumerable<SingleTarget<TNode0, TNode1, TNode2>> SelectedNodes(TBaseNode root)
            {
                return this.CodeGenerationEngine.GetDescendantNodes(root)
                           .OfType<TNode2>()
                           .SelectMany(grandparent => this.CodeGenerationEngine
                                                          .GetDescendantNodes(grandparent)
                                                          .OfType<TNode1>()
                                                          .SelectMany(parent => this.CodeGenerationEngine
                                                                                    .GetDescendantNodes(parent)
                                                                                    .OfType<TNode0>()
                                                                                    .Select(node => {
                                                                                        var result = new SingleTarget<TNode0, TNode1, TNode2>(this.CodeGenerationEngine, node, parent, grandparent);
                                                                                        foreach (var usingSelector in this.UsingSelectors)
                                                                                            result = (SingleTarget<TNode0, TNode1, TNode2>)usingSelector(result);
                                                                                        return result;
                                                                                    })))
                           .Where(x => WhereSelector(x));
            }

        }
    }
}
