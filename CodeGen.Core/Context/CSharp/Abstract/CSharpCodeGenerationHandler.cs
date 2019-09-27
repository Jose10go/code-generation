using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.Editing;
using System;
using static CodeGen.Context.CodeGenTypelessContext;

namespace CodeGen.Context.CSharp
{
    public partial class CSharpContextDocumentEditor : CSharpContext<DocumentEditor>
    {
        public abstract class RoslynDocumentEditorCommandHandler<TCommand,TSyntaxNode>:
            ICommandHandler<TCommand, ITarget<TSyntaxNode>,TSyntaxNode>
            where TCommand : ICommand<TSyntaxNode>
            where TSyntaxNode : CSharpSyntaxNode
        {
            public ITarget<TSyntaxNode> Target { get; set; }

            public abstract TCommand Command { get; set; }

            ITarget ICommandHandler.Target => Target;

            ICommand ICommandHandler.Command => Command;

            public abstract DocumentEditor ProcessDocument(DocumentEditor node);

            object ICommandHandler.ProcessDocument(object editor)
            {
                return ProcessDocument((DocumentEditor)editor);
            }
        }
    }
}