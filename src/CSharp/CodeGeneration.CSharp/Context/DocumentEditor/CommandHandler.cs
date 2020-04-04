using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.Editing;
namespace CodeGen.CSharp.Context.DocumentEdit
{
    public partial class CSharpContextDocumentEditor : CSharpContext<DocumentEditor>
    {
        public abstract class CommandHandler<TCommandBuilder,TSyntaxNode> : ICommandHandler<TCommandBuilder,TSyntaxNode>
            where TCommandBuilder : ICommand<TSyntaxNode>
            where TSyntaxNode : CSharpSyntaxNode
        {
            public TCommandBuilder Command { get; }
            protected CommandHandler(ICommand<TSyntaxNode> command)
            {
                this.Command = (TCommandBuilder)command;
            }

            public abstract void ProccessNode(TSyntaxNode node,DocumentEditor documentEditor);
        }
    }
}