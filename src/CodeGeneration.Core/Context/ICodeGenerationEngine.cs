using System.Collections.Generic;

namespace CodeGen.Context
{
    public abstract partial class CodeGenContext<TProject, TBaseNode, TRootNode, TSemanticModel> 
        where TRootNode:TBaseNode
    {
        public interface ICodeGenerationEngine:Core.ICodeGenerationEngine
        {
            TProject CurrentProject { get; set; }
            ICodeGenerationResolver CodeGenerationResolver { get; }
            //TODO: extract to a new interface
            IMultipleTarget<TSyntaxNode> Select<TSyntaxNode>()
                where TSyntaxNode : TBaseNode;
            IMultipleTarget<TSyntaxNode0,TSyntaxNode1> Select<TSyntaxNode0, TSyntaxNode1>()
                where TSyntaxNode0 : TBaseNode
                where TSyntaxNode1 : TBaseNode;
            IMultipleTarget<TSyntaxNode0,TSyntaxNode1,TSyntaxNode2> Select<TSyntaxNode0, TSyntaxNode1, TSyntaxNode2>()
                where TSyntaxNode0 : TBaseNode
                where TSyntaxNode1 : TBaseNode
                where TSyntaxNode2 : TBaseNode;

            ISingleTarget<TRootNode> SelectNew(string path);

            IEnumerable<string> GetAllDocumentsPaths(); 
            
            TRootNode GetRootNode(string Path);
        }
    }
}
