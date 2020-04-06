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
        public sealed class CSharpMultipleTarget<TSyntaxNode> : MultipleTarget<TSyntaxNode> 
            where TSyntaxNode : CSharpSyntaxNode
        {
            public CSharpMultipleTarget(ICodeGenerationEngine engine) : base(engine)
            {
            }

            public override IEnumerator<SingleTarget<TSyntaxNode>> GetEnumerator()
            {
                var project = this.CodeGenerationEngine.CurrentProject;
                var result = Enumerable.Empty<SingleTarget<TSyntaxNode>>();
                foreach (var documentid in project.DocumentIds)
                {
                    var document = project.GetDocument(documentid);
                    var root = document.GetSyntaxRootAsync().Result as CSharpSyntaxNode;
                    var semanticModel = document.GetSemanticModelAsync().Result;
                    var nodes = SelectedNodes(root, semanticModel);
                    result = result.Concat(nodes);
                }

                return result.GetEnumerator();
            }

            private IEnumerable<SingleTarget<TSyntaxNode>> SelectedNodes(CSharpSyntaxNode root, SemanticModel semanticModel)
            {
                return root.DescendantNodes()
                                   .OfType<TSyntaxNode>()
                                   .Select(node => new CSharpSingleTarget<TSyntaxNode>(this.CodeGenerationEngine, semanticModel, node))
                                   .Where(x => WhereSelector(x));
            }
        }

        public sealed class CSharpMultipleTarget<TSyntaxNode0,TSyntaxNode1> : MultipleTarget<TSyntaxNode0,TSyntaxNode1>
            where TSyntaxNode0 : CSharpSyntaxNode
            where TSyntaxNode1 : CSharpSyntaxNode
        {
            public CSharpMultipleTarget(ICodeGenerationEngine engine) : base(engine)
            {
            }

            public override IEnumerator<SingleTarget<TSyntaxNode0,TSyntaxNode1>> GetEnumerator()
            {
                var project = this.CodeGenerationEngine.CurrentProject;
                var result = Enumerable.Empty<SingleTarget<TSyntaxNode0,TSyntaxNode1>>();
                foreach (var documentid in project.DocumentIds)
                {
                    var document = project.GetDocument(documentid);
                    var root = document.GetSyntaxRootAsync().Result as CSharpSyntaxNode;
                    var semanticModel = document.GetSemanticModelAsync().Result;
                    var nodes = SelectedNodes(root, semanticModel);
                    result = result.Concat(nodes);
                }

                return result.GetEnumerator();
            }

            private IEnumerable<SingleTarget<TSyntaxNode0,TSyntaxNode1>> SelectedNodes(CSharpSyntaxNode root,SemanticModel semanticModel)
            {
                return root.DescendantNodes()
                           .OfType<TSyntaxNode1>()
                           .SelectMany(parent => parent.DescendantNodes()
                                                      .OfType<TSyntaxNode0>()
                                                      .Select(node => new CSharpSingleTarget<TSyntaxNode0, TSyntaxNode1>(this.CodeGenerationEngine, semanticModel, node, parent))
                                                      .Where(x => WhereSelector(x)));
            }
        }

        public sealed class CSharpMultipleTarget<TSyntaxNode0, TSyntaxNode1,TSyntaxNode2> : MultipleTarget<TSyntaxNode0, TSyntaxNode1,TSyntaxNode2>
            where TSyntaxNode0 : CSharpSyntaxNode
            where TSyntaxNode1 : CSharpSyntaxNode
            where TSyntaxNode2 : CSharpSyntaxNode
        {

            public CSharpMultipleTarget(ICodeGenerationEngine engine) : base(engine)
            {
            }

            public override IEnumerator<SingleTarget<TSyntaxNode0,TSyntaxNode1,TSyntaxNode2>> GetEnumerator()
            {
                var project = this.CodeGenerationEngine.CurrentProject;
                var result = Enumerable.Empty<SingleTarget<TSyntaxNode0, TSyntaxNode1, TSyntaxNode2>>();
                foreach (var documentid in project.DocumentIds)
                {
                    var document = project.GetDocument(documentid);
                    var root = document.GetSyntaxRootAsync().Result as CSharpSyntaxNode;
                    var semanticModel = document.GetSemanticModelAsync().Result;
                    var nodes = SelectedNodes(root, semanticModel);
                    result = result.Concat(nodes);
                }

                return result.GetEnumerator();
            }


            private IEnumerable<SingleTarget<TSyntaxNode0, TSyntaxNode1, TSyntaxNode2>> SelectedNodes(CSharpSyntaxNode root, SemanticModel semanticModel)
            {
                return root.DescendantNodes()
                           .OfType<TSyntaxNode2>()
                           .SelectMany(grandparent => grandparent.DescendantNodes()
                                                                .OfType<TSyntaxNode1>()
                                                                .SelectMany(parent => parent.DescendantNodes()
                                                                                            .OfType<TSyntaxNode0>()
                                                                                            .Select(node => new CSharpSingleTarget<TSyntaxNode0, TSyntaxNode1, TSyntaxNode2>(this.CodeGenerationEngine, semanticModel, node, parent, grandparent))
                                                                                            .Where(x => WhereSelector(x))));
            }

        }

        public sealed class CSharpSingleTarget<TSyntaxNode>:SingleTarget<TSyntaxNode>
            where TSyntaxNode:CSharpSyntaxNode
        {
            public CSharpSingleTarget(ICodeGenerationEngine engine, SemanticModel semanticModel,TSyntaxNode node):base(engine,node)
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

            public override ISymbol SemanticSymbol { get;}

            public override string DocumentPath { get; }
        }

        public sealed class CSharpSingleTarget<TSyntaxNode0, TSyntaxNode1> : SingleTarget<TSyntaxNode0,TSyntaxNode1>
            where TSyntaxNode0 : CSharpSyntaxNode
            where TSyntaxNode1 : CSharpSyntaxNode
        {
            public override ISymbol SemanticSymbol { get; }
            public override string DocumentPath { get; }
            public override SingleTarget<TSyntaxNode1> Parent { get; }

            public CSharpSingleTarget(ICodeGenerationEngine engine, SemanticModel semanticModel, TSyntaxNode0 node0, TSyntaxNode1 node1) : base(engine,node0)
            {
                this.SemanticSymbol = semanticModel.GetSymbolInfo(node0).Symbol;
                this.DocumentPath = node0.SyntaxTree.FilePath;
                Parent = new CSharpSingleTarget<TSyntaxNode1>(engine, semanticModel, node1);
            }
        }

        public sealed class CSharpSingleTarget<TSyntaxNode0, TSyntaxNode1, TSyntaxNode2> : SingleTarget<TSyntaxNode0,TSyntaxNode1,TSyntaxNode2>
            where TSyntaxNode0 : CSharpSyntaxNode
            where TSyntaxNode1 : CSharpSyntaxNode
            where TSyntaxNode2 : CSharpSyntaxNode
        {
            public override ISymbol SemanticSymbol { get; }
            public override string DocumentPath { get; }
            public override SingleTarget<TSyntaxNode1,TSyntaxNode2> Parent { get;}
            public CSharpSingleTarget(ICodeGenerationEngine engine, SemanticModel semanticModel, TSyntaxNode0 node0, TSyntaxNode1 node1,TSyntaxNode2 node2) : base(engine, node0)
            {
                this.SemanticSymbol = semanticModel.GetSymbolInfo(node0).Symbol;
                this.DocumentPath = node0.SyntaxTree.FilePath;
                Parent = new CSharpSingleTarget<TSyntaxNode1,TSyntaxNode2>(engine, semanticModel, node1,node2);
            }
        }

    }
}