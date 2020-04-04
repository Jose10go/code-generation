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

            public override IEnumerator<TSyntaxNode> GetEnumerator()
            {
                var project = this.CodeGenerationEngine.CurrentProject;
                var result = Enumerable.Empty<TSyntaxNode>();
                foreach (var documentid in project.DocumentIds)
                {
                    var document = project.GetDocument(documentid);
                    var root = document.GetSyntaxRootAsync().Result as CSharpSyntaxNode;
                    var semanticModel = document.GetSemanticModelAsync().Result;
                    result = result.Concat(SelectedNodes(root, semanticModel));
                }

                return  result.GetEnumerator();
            }

            virtual protected IEnumerable<TSyntaxNode> SelectedNodes(CSharpSyntaxNode root,SemanticModel semanticModel) 
            {
                return root.DescendantNodes()
                                   .OfType<TSyntaxNode>()
                                   .Where(x => WhereSelector(semanticModel.GetSymbolInfo(x).Symbol, x));
            }

        }

        public class CSharpTarget<TSyntaxNode0,TSyntaxNode1> : CSharpTarget<TSyntaxNode0> 
            where TSyntaxNode0 : CSharpSyntaxNode
            where TSyntaxNode1 : CSharpSyntaxNode
        {
            public CSharpTarget(ICodeGenerationEngine engine) : base(engine)
            {
            }

            protected override IEnumerable<TSyntaxNode0> SelectedNodes(CSharpSyntaxNode root, SemanticModel semanticModel)
            {
                return root.DescendantNodes()
                           .OfType<TSyntaxNode1>()//TODO: here parent filter
                           .SelectMany(parent=>base.SelectedNodes(parent,semanticModel));
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

            protected override IEnumerable<TSyntaxNode0> SelectedNodes(CSharpSyntaxNode root, SemanticModel semanticModel)
            {
                return root.DescendantNodes()
                           .OfType<TSyntaxNode2>()//TODO: here grand parent filter
                           .SelectMany(grandparent => base.SelectedNodes(grandparent,semanticModel));
            }
        }
    }
}