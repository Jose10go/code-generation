using CodeGen.Core;
using System;
using System.Collections.Generic;
namespace CodeGen.Context
{
    public abstract partial class CodeGenContext<TProject, TRootNode,TSemanticModel,TProcessEntity> 
    {
        public interface ITarget<TNode> : ITarget
            where TNode:TRootNode
        {
            Func<TSemanticModel,TNode, bool> WhereSelector{ get;set; }
            IEnumerable<TNode> Select(TRootNode root,Func<TNode,TSemanticModel> semanticModel);
            ICodeGenerationEngine CodeGenerationEngine { get; set; }
        }
    }
}
