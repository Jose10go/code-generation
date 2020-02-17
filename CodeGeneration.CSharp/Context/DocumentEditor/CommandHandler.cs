using CodeGen.Attributes;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.Editing;
using System.Linq;

namespace CodeGen.CSharp.Context.DocumentEdit
{
    public partial class CSharpContextDocumentEditor : CSharpContext<DocumentEditor>
    {
        public abstract class CommandHandler<TCommandBuilder,TSyntaxNode> : ICommandHandler<TCommandBuilder>
            where TCommandBuilder : ICommand<TSyntaxNode>
            where TSyntaxNode : CSharpSyntaxNode
        {
            public TCommandBuilder Command { get; }
            public CommandHandler(ICommand<TSyntaxNode> command)
            {
                this.Command = (TCommandBuilder)command;
            }

            public bool ProcessDocument(DocumentEditor processEntity)
            {
                var syntaxTreeroot= (CSharpSyntaxNode)processEntity.GetChangedRoot();
                var semantic=processEntity.SemanticModel;
                var nodes = Command.Target.Select(syntaxTreeroot,(node)=>semantic.GetSymbolInfo(node).Symbol);
                if (nodes.FirstOrDefault() is null)
                    return false;
                foreach (var item in nodes)
                    ProccesNode(item,processEntity);
                return true;
            }

            public abstract void ProccesNode(TSyntaxNode node,DocumentEditor documentEditor);
        }
    }
}