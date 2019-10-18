using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;

namespace CodeGen.Context.CSharp
{
    public partial class CSharpContext<TProcessEntity> : CodeGen.Context.CodeGenContext<Solution, CSharpSyntaxNode,TProcessEntity>
    {
        public interface ICSharpCodeGenerationEngine:ICodeGenerationEngine
        {
            new IChainCSharpTargetBuilder<TSyntaxNode> Select<TSyntaxNode>() where TSyntaxNode : CSharpSyntaxNode;
        }
    }
}
