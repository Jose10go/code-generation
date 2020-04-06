using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.Editing;
namespace CodeGen.CSharp.Context.DocumentEdit
{
    public partial class CSharpContextDocumentEditor : CSharpContext<DocumentEditor>
    {
        public abstract class CommandHandler<TCommand,TSyntaxNode, TOutput> : ICommandHandler<TCommand,TSyntaxNode,TOutput>
            where TCommand : ICommand<TSyntaxNode,TOutput>
            where TSyntaxNode : CSharpSyntaxNode
            where TOutput : CSharpSyntaxNode
        {
            public TCommand Command { get; }
            protected CommandHandler(ICommand<TSyntaxNode,TOutput> command)
            {
                this.Command = (TCommand)command;
            }

            public abstract SingleTarget<TOutput> ProccessNode(SingleTarget<TSyntaxNode> target,DocumentEditor documentEditor,ICodeGenerationEngine engine);
        }

        public abstract class CommandHandler<TCommand, TNode> : CommandHandler<TCommand, TNode, TNode>
            where TCommand : ICommand<TNode, TNode>
            where TNode : CSharpSyntaxNode
        {
            protected CommandHandler(ICommand<TNode,TNode> command):base(command)
            {
            }
        }
    }
}