using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.Editing;

namespace CodeGen.Context.CSharp.DocumentEdit
{
    public partial class CSharpContextDocumentEditor : CSharpContext<DocumentEditor>
    {
        public abstract class RoslynDocumentEditorCommandHandler<TCommand,TSyntaxNode>:
            ICommandHandler<TCommand>
            where TCommand : ICommand<TSyntaxNode>
            where TSyntaxNode : CSharpSyntaxNode
        {
            public abstract TCommand Command { get; set; }

            public abstract DocumentEditor ProcessDocument(DocumentEditor node);

        }
    }
}