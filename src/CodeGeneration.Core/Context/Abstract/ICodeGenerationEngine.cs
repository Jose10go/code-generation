using System;

namespace CodeGen.Context
{
    public abstract partial class CodeGenContext<TProject, TBaseNode, TRootNode, TSemanticModel, TProcessEntity> 
        where TRootNode:TBaseNode
    {
        public interface ICodeGenerationEngine:Core.ICodeGenerationEngine
        {
            TProject CurrentProject { get; }
            ICodeGenerationResolver CodeGenerationResolver { get; }
            //TODO: extract to a new interface
            MultipleTarget<TSyntaxNode> Select<TSyntaxNode>()
                where TSyntaxNode : TBaseNode;
            MultipleTarget<TSyntaxNode0,TSyntaxNode1> Select<TSyntaxNode0, TSyntaxNode1>()
                where TSyntaxNode0 : TBaseNode
                where TSyntaxNode1 : TBaseNode;
            MultipleTarget<TSyntaxNode0,TSyntaxNode1,TSyntaxNode2> Select<TSyntaxNode0, TSyntaxNode1, TSyntaxNode2>()
                where TSyntaxNode0 : TBaseNode
                where TSyntaxNode1 : TBaseNode
                where TSyntaxNode2 : TBaseNode;

            SingleTarget<TRootNode> SelectNew(string path);

            void UpdateProject(TProcessEntity processEntity);

            TProcessEntity GetProccesEntity<TNode>(SingleTarget<TNode> target)
                where TNode : TBaseNode;
        }
    }
}
