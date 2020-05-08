using CodeGen.Context;
using CodeGen.Core;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System;
using System.Linq;

namespace CodeGen.CSharp.Context
{
    public abstract partial class CSharpContext : CodeGenContext<Project, CSharpSyntaxNode, CompilationUnitSyntax, ISymbol>
    {
        public abstract class CSharpSingleTargeter<TSingleTarget,TNode> : SingleTargeter<TSingleTarget, TNode>
            where TSingleTarget : ISingleTargeter<TNode>
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
                var semanticModel = doc.GetSemanticModelAsync().Result;
                if(Node is MemberDeclarationSyntax)
                    _semanticSymbol =semanticModel.GetDeclaredSymbol(_node);
                else
                    _semanticSymbol =semanticModel.GetSymbolInfo(_node).Symbol;
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

            public IMultipleTarget<TNode0> AsMultiple()
            {
                return new MultipleTarget<TNode0>(Enumerable.Repeat(this,1));
            }

            public IMultipleTarget<TNode2, TNode1, TNode0> Select<TNode2, TNode1>()
                where TNode2: CSharpSyntaxNode
                where TNode1 : CSharpSyntaxNode
            {
                return this.Select<TNode1>().Select<TNode2>();
            }

            public IMultipleTarget<TNode1, TNode0> Select<TNode1>() 
                where TNode1 : CSharpSyntaxNode
            {
                return new MultipleTarget<TNode1, TNode0>(this.Node.DescendantNodes()
                                                                           .OfType<TNode1>()
                                                                           .Select(x => new CSharpSingleTarget<TNode1, TNode0>(this.CodeGenerationEngine, Guid.NewGuid(), this, this.DocumentPath))
                                                                           .ToList());
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

            public IMultipleTarget<TNode0, TNode1> AsMultiple()
            {
                return new MultipleTarget<TNode0,TNode1>(Enumerable.Repeat(this,1));
            }

            public IMultipleTarget<TNode, TNode0, TNode1> Select<TNode>()
                where TNode : CSharpSyntaxNode
            {
                return new MultipleTarget<TNode, TNode0, TNode1>(this.Node.DescendantNodes()
                                                                        .OfType<TNode>()
                                                                        .Select(x => new CSharpSingleTarget<TNode, TNode0, TNode1>(this.CodeGenerationEngine,Guid.NewGuid(),this,this.DocumentPath))
                                                                        .ToList());
            }
        }

        public sealed class CSharpSingleTarget<TNode0, TNode1, TNode2> : CSharpSingleTargeter<ISingleTarget<TNode0, TNode1, TNode2>, TNode0>, ISingleTarget<TNode0, TNode1, TNode2>
            where TNode0 : CSharpSyntaxNode
            where TNode1 : CSharpSyntaxNode
            where TNode2 : CSharpSyntaxNode
        {
            public ISingleTarget<TNode1, TNode2> Parent { get; }
            public ISingleTarget<TNode2> Grandparent => Parent.Parent;

            internal CSharpSingleTarget(ICodeGenerationEngine engine, Guid id, CSharpSingleTarget<TNode1,TNode2> parent, string path) : base(engine,id, path)
            {
                this.Parent = parent;
            }

            public IMultipleTarget<TNode0, TNode1, TNode2> AsMultiple()
            {
                return new MultipleTarget<TNode0, TNode1, TNode2>(Enumerable.Repeat(this, 1));
            }
        }

    }
}
