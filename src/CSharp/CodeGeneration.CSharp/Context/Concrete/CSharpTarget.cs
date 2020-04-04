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

            public override IEnumerator<ISingleTarget<TSyntaxNode>> GetEnumerator()
            {
                var project = this.CodeGenerationEngine.CurrentProject;
                var result = Enumerable.Empty<ISingleTarget<TSyntaxNode>>();
                foreach (var documentid in project.DocumentIds)
                {
                    var document = project.GetDocument(documentid);
                    var root = document.GetSyntaxRootAsync().Result as CSharpSyntaxNode;
                    var semanticModel = document.GetSemanticModelAsync().Result;
                    var nodes = SelectedNodes(root, semanticModel);
                    result = result.Concat(nodes);
                }

                return  result.GetEnumerator();
            }

            protected virtual IEnumerable<ISingleTarget<TSyntaxNode>> SelectedNodes(CSharpSyntaxNode root,SemanticModel semanticModel)
            {
                return root.DescendantNodes()
                                   .OfType<TSyntaxNode>()
                                   .Select(node=>new CSharpSingleTarget<TSyntaxNode>(this.CodeGenerationEngine,node,semanticModel))
                                   .Where(x => WhereSelector(x));
            }

        }

        public class CSharpTarget<TSyntaxNode0,TSyntaxNode1> : CSharpTarget<TSyntaxNode0> 
            where TSyntaxNode0 : CSharpSyntaxNode
            where TSyntaxNode1 : CSharpSyntaxNode
        {
            public CSharpTarget(ICodeGenerationEngine engine) : base(engine)
            {
            }

            protected override IEnumerable<ISingleTarget<TSyntaxNode0>> SelectedNodes(CSharpSyntaxNode root,SemanticModel semanticModel)
            {
                return root.DescendantNodes()
                           .OfType<TSyntaxNode1>()
                           .Select(node=>new CSharpSingleTarget<TSyntaxNode1>(this.CodeGenerationEngine,node,semanticModel))
                           //TODO: here parent filter
                           .SelectMany(parent=>base.SelectedNodes(parent.Node,semanticModel));
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

            protected override IEnumerable<ISingleTarget<TSyntaxNode0>> SelectedNodes(CSharpSyntaxNode root,SemanticModel semanticModel)
            {
                return root.DescendantNodes()
                           .OfType<TSyntaxNode2>()//TODO: here grand parent filter
                           .Select(node => new CSharpSingleTarget<TSyntaxNode2>(this.CodeGenerationEngine, node, semanticModel))
                           .SelectMany(grandparent => base.SelectedNodes(grandparent.Node,semanticModel));
            }
        }

        public class CSharpSingleTarget<TSyntaxNode>:SingleTarget<TSyntaxNode>
            where TSyntaxNode:CSharpSyntaxNode
        {
            public CSharpSingleTarget(ICodeGenerationEngine engine,TSyntaxNode node, SemanticModel semanticModel) :base(engine,node)
            {
                this.SemanticSymbol = semanticModel.GetSymbolInfo(node).Symbol;
                this.DocumentPath = node.SyntaxTree.FilePath;
                //TODO: get specific symbol info by casting
                //switch (.Kind)
                //{
                //    case SymbolKind.Alias:
                //        this.SemanticSymbol =
                //        break;
                //    case SymbolKind.ArrayType:
                //        break;
                //    case SymbolKind.Assembly:
                //        break;
                //    case SymbolKind.DynamicType:
                //        break;
                //    case SymbolKind.ErrorType:
                //        break;
                //    case SymbolKind.Event:
                //        break;
                //    case SymbolKind.Field:
                //        break;
                //    case SymbolKind.Label:
                //        break;
                //    case SymbolKind.Local:
                //        break;
                //    case SymbolKind.Method:
                //        break;
                //    case SymbolKind.NetModule:
                //        break;
                //    case SymbolKind.NamedType:
                //        break;
                //    case SymbolKind.Namespace:
                //        break;
                //    case SymbolKind.Parameter:
                //        break;
                //    case SymbolKind.PointerType:
                //        break;
                //    case SymbolKind.Property:
                //        break;
                //    case SymbolKind.RangeVariable:
                //        break;
                //    case SymbolKind.TypeParameter:
                //        break;
                //    case SymbolKind.Preprocessing:
                //        break;
                //    case SymbolKind.Discard:
                //        break;
                //    default:
                //        break;
                //}
            }

            public override ISymbol SemanticSymbol { get; }
            public override string DocumentPath { get; }
        }
    }
}