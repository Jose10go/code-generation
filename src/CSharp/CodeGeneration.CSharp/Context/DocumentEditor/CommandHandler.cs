using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.Editing;

namespace CodeGen.CSharp.Context.DocumentEdit
{
    public partial class CSharpContextDocumentEditor : CSharpContext<DocumentEditor>
    {
        public abstract class CommandHandler<TCommandBuilder,TSyntaxNode> : ICommandHandler<TCommandBuilder>
            where TCommandBuilder : ICommand<TSyntaxNode>
            where TSyntaxNode : CSharpSyntaxNode
        {
            public TCommandBuilder Command { get; }
            protected CommandHandler(ICommand<TSyntaxNode> command)
            {
                this.Command = (TCommandBuilder)command;
            }

            public bool ProcessDocument(DocumentEditor processEntity)
            {
                bool hasAny = false;
                //Not need to implement IEnumerable to use in a foreach statement.
                //Only need to have a public GetEnumeratorMethod().
                //Wich allows to avoid the unwanted use of linq in Targets
                foreach (var item in Command.Target)
                {
                    hasAny = true;
                    ProccesNode(item, processEntity);
                }

                return hasAny;
            }

            public abstract void ProccesNode(TSyntaxNode node,DocumentEditor documentEditor);
        }
    }
}