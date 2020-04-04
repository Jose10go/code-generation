using System;

namespace CodeGen.Context
{
    public abstract partial class CodeGenContext<TProject, TBaseNode, TRootNode, TSemanticModel, TProcessEntity> 
        where TRootNode:TBaseNode
    {
        public interface ICodeGenerationEngine:Core.ICodeGenerationEngine
        {
            TProject CurrentProject { get; }

            Target<TSyntaxNode> Select<TSyntaxNode>() 
                where TSyntaxNode : TBaseNode;
            Target<TSyntaxNode0> Select<TSyntaxNode0, TSyntaxNode1>() 
                where TSyntaxNode0 : TBaseNode
                where TSyntaxNode1 : TBaseNode;
            Target<TSyntaxNode0> Select<TSyntaxNode0, TSyntaxNode1,TSyntaxNode2>()
                where TSyntaxNode0 : TBaseNode
                where TSyntaxNode1 : TBaseNode
                where TSyntaxNode2 : TBaseNode;

            Target<TRootNode> SelectNew(string path);

            TCommand Execute<TCommand, TNode>(ITarget<TNode> target, Func<TCommand, TCommand> commandModifiers)
                where TCommand : ICommand<TNode>
                where TNode : TBaseNode;
        }
    }
}
