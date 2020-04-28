using CodeGen.Core;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CodeGen.Context
{
    public abstract partial class CodeGenContext<TProject, TBaseNode, TRootNode, TSemanticModel>
        where TRootNode : TBaseNode
    {
        public interface IUsing<InTarget,OutTarget,TNode>
            where InTarget:Core.ITarget
            where OutTarget:Core.ITarget
            where TNode:TBaseNode
        {
            OutTarget Using<T>(Func<InTarget, T> usingSelector, out Key<T> key);
        }

        public interface IWhere<InTarget, OutTarget, TNode>
            where InTarget : Core.ITarget
            where OutTarget : Core.ITarget
            where TNode : TBaseNode
        {
            OutTarget Where(Func<InTarget, bool> whereSelector);
        }

        public interface ITargetGet<OutTarget> 
            where OutTarget:Core.ITarget
        {
            OutTarget Get<T>(Key<T> key, out T value);
        }

        public interface ISingleTargeter<TNode>:Core.ISingleTarget
            where TNode:TBaseNode
        {
            Guid Id { get; }
            TNode Node { get; }
            TSemanticModel SemanticSymbol { get; }
            string DocumentPath { get; }
            ISingleTarget<TOutNode> Execute<TCommand, TOutNode>(Func<TCommand, ICommandResult<TOutNode>> commandModifiers)
                where TCommand : ICommandOn<TNode>, ICommandResult<TOutNode>
                where TOutNode : TBaseNode;
        }

        public interface ISingleTarget<TNode>:ISingleTargeter<TNode>,IUsing<ISingleTarget<TNode>, ISingleTarget<TNode>,TNode>, ITargetGet<ISingleTarget<TNode>>
            where TNode:TBaseNode
        {
            
        }

        public interface ISingleTarget<TNode0,TNode1> : ISingleTargeter<TNode0>, IUsing<ISingleTarget<TNode0,TNode1>, ISingleTarget<TNode0,TNode1>, TNode0>,ITargetGet<ISingleTarget<TNode0,TNode1>>
            where TNode0 : TBaseNode
            where TNode1 : TBaseNode
        {
            ISingleTarget<TNode1> Parent { get; }
        }

        public interface ISingleTarget<TNode0, TNode1,TNode2> : ISingleTargeter<TNode0>, IUsing<ISingleTarget<TNode0, TNode1, TNode2>, ISingleTarget<TNode0,TNode1,TNode2>, TNode0>,ITargetGet<ISingleTarget<TNode0,TNode1,TNode2>>
            where TNode0 : TBaseNode
            where TNode1 : TBaseNode
            where TNode2 : TBaseNode
        {
            ISingleTarget<TNode1,TNode2> Parent { get; }
            ISingleTarget<TNode2> Grandparent { get; }
        }
        
        public abstract class SingleTargeter<TSingleTarget,TNode>:ISingleTargeter<TNode>,ITargetGet<TSingleTarget>,IUsing<TSingleTarget,TSingleTarget,TNode>
            where TSingleTarget:ISingleTargeter<TNode>
            where TNode:TBaseNode
        {
            public abstract TNode Node { get; }
            public abstract TSemanticModel SemanticSymbol { get; }
            public string DocumentPath { get;}
            
            protected ICodeGenerationEngine CodeGenerationEngine { get; }
            private readonly Dictionary<object, Func<TSingleTarget, object>> dictionary;
            public Guid Id { get; }
            protected SingleTargeter(ICodeGenerationEngine engine, Guid id, string path)
            {
                this.CodeGenerationEngine = engine;
                this.DocumentPath = path;
                this.dictionary = new Dictionary<object, Func<TSingleTarget, object>>();
                this.Id = id;
            }

            public TSingleTarget Get<T>(Key<T> key, out T value)
            {
                TSingleTarget self = (TSingleTarget)(ISingleTargeter<TNode>)this;
                value = (T)dictionary[key](self);
                return self;
            }

            public TSingleTarget Using<T>(Func<TSingleTarget,T> usingSelector, out Key<T> key)
            {
                TSingleTarget self = (TSingleTarget)(ISingleTargeter<TNode>)this;
                key = new Key<T>(usingSelector.GetHashCode());//TODO: another way to generate a key
                this.dictionary.Add(key,(target)=>usingSelector(target));
                return self;
            }

            public ISingleTarget<TOutput> Execute<TCommand, TOutput>(Func<TCommand, ICommandResult<TOutput>> commandModifiers)
                where TCommand : ICommandOn<TNode>,ICommandResult<TOutput>
                where TOutput : TBaseNode
            {
                var command = this.CodeGenerationEngine.CodeGenerationResolver.ResolveCommand<TCommand>();
                var handler = this.CodeGenerationEngine.CodeGenerationResolver.ResolveCommandHandler(command);
                var singleTarget = this as ISingleTargeter<TNode>;
                command.SingleTarget = singleTarget;
                commandModifiers(command);

                return handler.ProccessTarget<TCommand,TNode,TOutput>(singleTarget,this.CodeGenerationEngine);
            }

            void ISingleTarget.Get<T>(Key<T> key, out T value)
            {
                this.Get(key, out value);
            }
        }

        public interface IMultipleTargeter<TNode> : Core.IMultipleTarget 
            where TNode:TBaseNode
        {
            IMultipleTarget<TOutNode> Execute<TCommand, TOutNode>(Func<TCommand, ICommandResult<TOutNode>> commandModifiers)
                where TCommand : ICommandOn<TNode>, ICommandResult<TOutNode>
                where TOutNode : TBaseNode;
        }
        
        public interface IMultipleTarget<TNode> : IMultipleTargeter<TNode>,
                                                  IUsing<ISingleTarget<TNode>,IMultipleTarget<TNode>,TNode>,
                                                  IWhere<ISingleTarget<TNode>,IMultipleTarget<TNode>,TNode>
            where TNode : TBaseNode
        {
            IEnumerator<ISingleTarget<TNode>> GetEnumerator();
        }

        public interface IMultipleTarget<TNode0,TNode1> : IMultipleTargeter<TNode0>,
                                                          IUsing<ISingleTarget<TNode0,TNode1>,IMultipleTarget<TNode0,TNode1>,TNode0>,
                                                          IWhere<ISingleTarget<TNode0,TNode1>,IMultipleTarget<TNode0,TNode1>,TNode0>
            where TNode0 : TBaseNode
            where TNode1 : TBaseNode
        {
            IEnumerator<ISingleTarget<TNode0,TNode1>> GetEnumerator();
        }

        public interface IMultipleTarget<TNode0, TNode1,TNode2> : IMultipleTargeter<TNode0>,
                                                                  IUsing<ISingleTarget<TNode0, TNode1,TNode2>, IMultipleTarget<TNode0, TNode1,TNode2>, TNode0>,
                                                                  IWhere<ISingleTarget<TNode0, TNode1,TNode2>, IMultipleTarget<TNode0, TNode1,TNode2>, TNode0>
            where TNode0 : TBaseNode
            where TNode1 : TBaseNode
            where TNode2 : TBaseNode
        {
            IEnumerator<ISingleTarget<TNode0,TNode1,TNode2>> GetEnumerator();
        }

        public abstract class MultipleTargeter<MultipleTarget, SingleTarget, TNode>:IMultipleTargeter<TNode>, 
                                                                                    IUsing<SingleTarget,MultipleTarget,TNode>,
                                                                                    IWhere<SingleTarget,MultipleTarget,TNode>
            where MultipleTarget : IMultipleTargeter<TNode>
            where SingleTarget : ISingleTargeter<TNode>,IUsing<SingleTarget,SingleTarget,TNode>
            where TNode : TBaseNode
        {
            protected MultipleTargeter(IEnumerable<SingleTarget> singleTargets)
            {
                this.SingleTargets = singleTargets;
            }

            IEnumerable<SingleTarget> SingleTargets { get; set; }
            public MultipleTarget Where(Func<SingleTarget, bool> whereSelector)
            {
                this.SingleTargets = SingleTargets.Where(whereSelector);
                return (MultipleTarget)(IMultipleTargeter<TNode>)this;
            }

            //Not need to implement IEnumerable to use in a foreach statement.
            //Only need to have a public GetEnumeratorMethod().
            //Wich allows to avoid the unwanted use of linq in Targets.

            public IEnumerator<SingleTarget> GetEnumerator()
            {
                return SingleTargets.GetEnumerator();
            }
            
            public IMultipleTarget<TOutput> Execute<TCommand,TOutput>(Func<TCommand,ICommandResult<TOutput>> commandModifiers)
                where TCommand : ICommandOn<TNode>, ICommandResult<TOutput>
                where TOutput : TBaseNode
            {
                var output = Enumerable.Empty<ISingleTarget<TOutput>>();
                foreach (var singleTarget in this)
                    output=output.Append(singleTarget.Execute(commandModifiers));

                return new MultipleTarget<TOutput>(output);
            }

            public MultipleTarget Using<T>(Func<SingleTarget, T> usingSelector, out Key<T> key)
            {
                key = new Key<T>(usingSelector.GetHashCode());
                this.SingleTargets = SingleTargets.Select(x => x.Using(usingSelector, out var _));
                return (MultipleTarget)(IMultipleTargeter<TNode>)this;
            }

            //public static MultipleTarget From(ICodeGenerationEngine engine,params IEnumerable<ISingleTarget<TNode>>[] targets)
            //{
            //    var result = Enumerable.Empty<ISingleTarget<TNode>>();
            //    foreach (var item in targets)
            //        result = result.Concat(item);
            //    return (MultipleTarget)(IMultipleTarget<TNode>)new MultipleTarget() { };
            //}

            //public static MultipleTarget From(ICodeGenerationEngine engine, params SingleTarget[] targets)
            //{
            //    return (MultipleTarget)(ITarget<TNode>)new MultipleTargeter<MultipleTarget, SingleTarget, TNode>(engine, targets.AsEnumerable());
            //}

            //public static MultipleTarget From(ICodeGenerationEngine engine, IEnumerable<SingleTarget> enumerable, params SingleTarget[] targets)
            //{
            //    var result = enumerable;
            //    foreach (var item in targets)
            //        result = result.Append(item);
            //    return (MultipleTarget)(ITarget<TNode>)new MultipleTargeter<MultipleTarget, SingleTarget, TNode>(engine, result);
            //}

        }

        public sealed class MultipleTarget<TNode> : MultipleTargeter<IMultipleTarget<TNode>, ISingleTarget<TNode>, TNode>,
                                                    IMultipleTarget<TNode>
           where TNode : TBaseNode
        {
            public MultipleTarget():base(Enumerable.Empty<ISingleTarget<TNode>>()) 
            {
            }
            
            public MultipleTarget(IEnumerable<ISingleTarget<TNode>> singleTargets)
                : base(singleTargets)
            {
            }

        }

        public sealed class MultipleTarget<TNode, TNode1> : MultipleTargeter<IMultipleTarget<TNode, TNode1>, ISingleTarget<TNode, TNode1>, TNode>,
                                                            IMultipleTarget<TNode, TNode1>
             where TNode : TBaseNode
             where TNode1 : TBaseNode
        {
            public MultipleTarget() : base(Enumerable.Empty<ISingleTarget<TNode,TNode1>>())
            {
            }
            public MultipleTarget(IEnumerable<ISingleTarget<TNode, TNode1>> singleTargets)
                : base(singleTargets)
            {
            }
            
        }

        public sealed class MultipleTarget<TNode, TNode1, TNode2> : MultipleTargeter<IMultipleTarget<TNode, TNode1, TNode2>, ISingleTarget<TNode, TNode1, TNode2>, TNode>,
                                                                    IMultipleTarget<TNode, TNode1, TNode2>
             where TNode : TBaseNode
             where TNode1 : TBaseNode
             where TNode2 : TBaseNode
        {
            public MultipleTarget() : base(Enumerable.Empty<ISingleTarget<TNode,TNode1,TNode2>>())
            {
            }
            public MultipleTarget(IEnumerable<ISingleTarget<TNode, TNode1, TNode2>> singleTargets)
                : base(singleTargets)
            {
            }

        }

    }
}
