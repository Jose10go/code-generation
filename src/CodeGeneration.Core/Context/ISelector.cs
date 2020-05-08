using CodeGen.Core;

namespace CodeGen.Context
{
    public partial class CodeGenContext<TProject, TBaseNode, TRootNode, TSemanticModel>
        where TRootNode:TBaseNode
    {
        public interface ISelector 
        {
            IMultipleTarget<TSyntaxNode> Select<TSyntaxNode>()
                where TSyntaxNode : TBaseNode;
            IMultipleTarget<TSyntaxNode0, TSyntaxNode1> Select<TSyntaxNode0, TSyntaxNode1>()
                where TSyntaxNode0 : TBaseNode
                where TSyntaxNode1 : TBaseNode;
            IMultipleTarget<TSyntaxNode0, TSyntaxNode1, TSyntaxNode2> Select<TSyntaxNode0, TSyntaxNode1, TSyntaxNode2>()
                where TSyntaxNode0 : TBaseNode
                where TSyntaxNode1 : TBaseNode
                where TSyntaxNode2 : TBaseNode;
        }

        public interface ISelector<TNode1,TNode2> 
            where TNode1 : TBaseNode
            where TNode2 : TBaseNode
        {
            IMultipleTarget<TNode0, TNode1, TNode2> Select<TNode0>()
               where TNode0 : TBaseNode;
        }

        public interface ISelector<TNode>
            where TNode : TBaseNode
        {
            IMultipleTarget<TNode0, TNode1, TNode> Select<TNode0, TNode1>()
               where TNode0 : TBaseNode
               where TNode1 : TBaseNode;
         
            IMultipleTarget<TNode0, TNode> Select<TNode0>()
               where TNode0 : TBaseNode;
        }

    }
}