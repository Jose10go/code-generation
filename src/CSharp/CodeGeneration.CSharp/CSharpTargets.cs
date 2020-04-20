﻿using CodeGen.Context;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System;
using System.Linq;

namespace CodeGen.CSharp.Context
{
    public abstract partial class CSharpContext<TProcessEntity> : CodeGenContext<Project, CSharpSyntaxNode, CompilationUnitSyntax, ISymbol, TProcessEntity>
    {
        public abstract class CSharpSingleTargeter<TSingleTarget,TNode> : SingleTargeter<TSingleTarget, TNode>
            where TSingleTarget : ISingleTarget<TNode>
            where TNode : CSharpSyntaxNode
        {
            TNode _node;
            ISymbol _semanticSymbol;
            public override TNode Node => GetNode();
            public override ISymbol SemanticSymbol => GetSymbol();

            private ISymbol GetSymbol()
            {
                if (_node is null || this.CodeGenerationEngine.CurrentProject.GetDocument(_node.SyntaxTree) is null)
                    Update();
                return _semanticSymbol;
            }

            private TNode GetNode()
            {
                if (_node is null || this.CodeGenerationEngine.CurrentProject.GetDocument(_node.SyntaxTree) is null)
                    Update();
                return _node;
            }

            private void Update() 
            {
                var _root=CodeGenerationEngine.GetRootNode(this.DocumentPath);
                _node = _root.GetAnnotatedNodes($"{this.Id}").First() as TNode;
                var doc = this.CodeGenerationEngine.CurrentProject.DocumentIds.Select(id => this.CodeGenerationEngine.CurrentProject.GetDocument(id))
                                                   .First(doc => doc.FilePath == this.DocumentPath);
                _semanticSymbol=doc.GetSemanticModelAsync().Result.GetSymbolInfo(_node).Symbol;
            }

            protected CSharpSingleTargeter(ICodeGenerationEngine engine,Guid id,string path) : base(engine,id, path)
            {
            }

        }

        public sealed class CSharpSingleTarget<TNode0> : CSharpSingleTargeter<ISingleTarget<TNode0>, TNode0>,ISingleTarget<TNode0>
            where TNode0 : CSharpSyntaxNode
        {
            public CSharpSingleTarget(ICodeGenerationEngine engine, Guid id,string path) : base(engine,id,path)
            {
            }
        }

        public sealed class CSharpSingleTarget<TNode0, TNode1> : CSharpSingleTargeter<ISingleTarget<TNode0, TNode1>, TNode0>, ISingleTarget<TNode0, TNode1>
            where TNode0 : CSharpSyntaxNode
            where TNode1 : CSharpSyntaxNode
        {
            public ISingleTarget<TNode1> Parent { get; }

            internal CSharpSingleTarget(ICodeGenerationEngine engine, Guid id, CSharpSingleTarget<TNode1> parent, string path) : base(engine,id,path)
            {
                this.Parent = parent;
            }

            ISingleTarget<TNode0> ITargetGet<ISingleTarget<TNode0>>.Get<T>(Key<T> key, out T value)
            {
                return this.Get(key, out value);
            }

            ISingleTarget<TNode0> IUsing<ISingleTarget<TNode0>, ISingleTarget<TNode0>, TNode0>.Using<T>(Func<ISingleTarget<TNode0>, T> usingSelector, out Key<T> key)
            {
                return this.Using(usingSelector,out key);
            }
        }

        public sealed class CSharpSingleTarget<TNode0, TNode1, TNode2> : CSharpSingleTargeter<ISingleTarget<TNode0, TNode1, TNode2>, TNode0>, ISingleTarget<TNode0, TNode1, TNode2>
            where TNode0 : CSharpSyntaxNode
            where TNode1 : CSharpSyntaxNode
            where TNode2 : CSharpSyntaxNode
        {
            public ISingleTarget<TNode1, TNode2> Parent { get; }
            public ISingleTarget<TNode2> Grandparent => Parent.Parent;
            ISingleTarget<TNode1> ISingleTarget<TNode0, TNode1>.Parent => this.Parent;

            internal CSharpSingleTarget(ICodeGenerationEngine engine, Guid id, CSharpSingleTarget<TNode1,TNode2> parent, string path) : base(engine,id, path)
            {
                this.Parent = parent;
            }

            ISingleTarget<TNode0> ITargetGet<ISingleTarget<TNode0>>.Get<T>(Key<T> key, out T value)
            {
                return this.Get(key, out value);
            }

            ISingleTarget<TNode0, TNode1> ITargetGet<ISingleTarget<TNode0, TNode1>>.Get<T>(Key<T> key, out T value)
            {
                return this.Get(key, out value);
            }

            ISingleTarget<TNode0> IUsing<ISingleTarget<TNode0>, ISingleTarget<TNode0>, TNode0>.Using<T>(Func<ISingleTarget<TNode0>, T> usingSelector, out Key<T> key)
            {
                return this.Using(usingSelector, out key);
            }

            ISingleTarget<TNode0, TNode1> IUsing<ISingleTarget<TNode0, TNode1>, ISingleTarget<TNode0, TNode1>, TNode0>.Using<T>(Func<ISingleTarget<TNode0, TNode1>, T> usingSelector, out Key<T> key)
            {
                return this.Using(usingSelector, out key);
            }
        }

    }
}
