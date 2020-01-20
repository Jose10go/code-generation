using CodeGen.Core;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.Editing;

namespace CodeGen.CSharp.Context.DocumentEdit
{
    public partial class CSharpContextDocumentEditor : CSharpContext<DocumentEditor>
    {
        public abstract class RoslynDocumentEditorCommandHandler<TSyntaxNode>:
            ICommandHandler
            where TSyntaxNode : CSharpSyntaxNode
        {
            public abstract Command Command { get; set; }
            public abstract void ProcessDocument(DocumentEditor processEntity);
        }
    }
}