using System;
using System.Collections.Generic;
using static CodeGen.Context.CodeGenTypelessContext;

namespace CodeGen.Context
{
    // Typed context - Abstract
    public partial class CodeGenContext<TProject, TRootNode, TProcessEntity> 
    {
        public interface ITarget<TNode> : ITarget
        {
            Func<TNode, bool> WhereSelector{ get;set; }
            IEnumerable<TNode> Select(TRootNode root);
        }
    }
}
