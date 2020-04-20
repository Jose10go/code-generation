using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.Editing;
using Microsoft.CodeAnalysis;
using System;

namespace CodeGen.CSharp.Context.DocumentEdit
{
    public partial class CSharpContextDocumentEditor : CSharpContext<DocumentEditor>
    {
        public abstract class CommandHandler<TCommand, TSyntaxNode, TOutput> : ICommandHandler<TCommand, TSyntaxNode, TOutput>
            where TCommand : ICommand<TSyntaxNode,TOutput>
            where TSyntaxNode : CSharpSyntaxNode
            where TOutput : CSharpSyntaxNode
        {
            public TCommand Command { get; }
            protected CommandHandler(Core.ICommand command)
            {
                this.Command = (TCommand)command;
            }

            public ISingleTarget<TOutput> ProccessTarget(ISingleTarget<TSyntaxNode> target, ICodeGenerationEngine engine)
            {
                var doc = engine.CurrentProject.GetDocument(target.Node.SyntaxTree);
                DocumentEditor editor = DocumentEditor.CreateAsync(doc).Result;//TODO: make async???
                var id = Guid.NewGuid();
                var outNode = ProccessNode(target.Node, editor,id);
                var result = new CSharpSingleTarget<TOutput>(engine, id, doc.FilePath);
                var document = editor.GetChangedDocument();
                engine.CurrentProject = document.Project;
                return result;
            }
            
            protected abstract TOutput ProccessNode(TSyntaxNode node,DocumentEditor documentEditor,Guid id);
        }

        public abstract class CommandHandler<TCommand, TNode> : CommandHandler<TCommand, TNode, TNode>
            where TCommand : ICommand<TNode,TNode>
            where TNode : CSharpSyntaxNode
        {
            protected CommandHandler(Core.ICommand command):base(command)
            {
            }
        }
    }
}