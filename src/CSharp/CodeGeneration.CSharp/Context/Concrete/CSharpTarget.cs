using System;
using System.Collections.Generic;
using System.Linq;
using CodeGen.Context;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CodeGen.CSharp.Context
{
    public abstract partial class CSharpContext<TProcessEntity> : CodeGenContext<Project, CSharpSyntaxNode,CompilationUnitSyntax,ISymbol, TProcessEntity>
    {
        public class CSharpTarget<TSyntaxNode> : Target<TSyntaxNode> where TSyntaxNode : CSharpSyntaxNode
        {
            public CSharpTarget(ICodeGenerationEngine engine) : base(engine)
            {
            }
            public override IEnumerable<TSyntaxNode> Select(CSharpSyntaxNode root,Func<TSyntaxNode,ISymbol> semanticModelSelector)
            {
                return root.DescendantNodes()
                           .OfType<TSyntaxNode>()
                           .Where(x=>WhereSelector(semanticModelSelector(x),x));
            }
        }

        public class CSharpTarget<TSyntaxNode0,TSyntaxNode1> : CSharpTarget<TSyntaxNode0> 
            where TSyntaxNode0 : CSharpSyntaxNode
            where TSyntaxNode1 : CSharpSyntaxNode
        {
            public CSharpTarget(ICodeGenerationEngine engine) : base(engine)
            {
            }

            public override IEnumerable<TSyntaxNode0> Select(CSharpSyntaxNode root, Func<TSyntaxNode0, ISymbol> semanticModelSelector)
            {
                return root.DescendantNodes()
                           .OfType<TSyntaxNode1>()//TODO: here parent filter
                           .SelectMany(
                                parent => parent.DescendantNodes()
                                                .OfType<TSyntaxNode0>()
                                                .Where(x => WhereSelector(semanticModelSelector(x), x)));
            }
        }

        public class CSharpTarget<TSyntaxNode0, TSyntaxNode1,TSyntaxNode2> : CSharpTarget<TSyntaxNode0,TSyntaxNode1>
            where TSyntaxNode0 : CSharpSyntaxNode
            where TSyntaxNode1 : CSharpSyntaxNode
            where TSyntaxNode2 : CSharpSyntaxNode
        {
            public CSharpTarget(ICodeGenerationEngine engine) : base(engine)
            {
            }

            public override IEnumerable<TSyntaxNode0> Select(CSharpSyntaxNode root, Func<TSyntaxNode0, ISymbol> semanticModelSelector)
            {
                return root.DescendantNodes()
                           .OfType<TSyntaxNode2>()//TODO: here grand parent filter
                           .SelectMany(
                                grandparent => grandparent.DescendantNodes()
                                                          .OfType<TSyntaxNode1>()
                                                          .SelectMany(
                                                             parent => parent.DescendantNodes()//TODO: here parent filter
                                                                             .OfType<TSyntaxNode0>()
                                                                             .Where(x => WhereSelector(semanticModelSelector(x), x))));
            }
        }
    }
}