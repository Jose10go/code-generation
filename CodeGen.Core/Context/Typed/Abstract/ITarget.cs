using System.Collections.Generic;
using static CodeGen.Context.CodeGenTypelessContext;

namespace CodeGen.Context
{
    // Typed context - Abstract
    public partial class CodeGenContext<TProject, TRootNode, TProcessEntity> 
    {
        public interface ITarget<out TNode> : ITarget
        {
            //new Func<TNode, bool> WhereSelector { get; }//TODO check without func because of in Tnode

            IEnumerable<TNode> Select(TRootNode root);
        }
    }
}
