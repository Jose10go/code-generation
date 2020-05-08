using System.Collections.Generic;

namespace CodeGen.Context
{
    public abstract partial class CodeGenContext<TProject, TBaseNode, TRootNode, TSemanticModel> 
        where TRootNode:TBaseNode
    {
        public interface ICodeGenerationEngine:Core.ICodeGenerationEngine,ISelector
        {
            TProject CurrentProject { get; set; }
            ICodeGenerationResolver CodeGenerationResolver { get; }

            ISingleTarget<TRootNode> SelectNew(string path);

            IEnumerable<string> GetAllDocumentsPaths(); 
            
            TRootNode GetRootNode(string Path);
        }
    }
}
