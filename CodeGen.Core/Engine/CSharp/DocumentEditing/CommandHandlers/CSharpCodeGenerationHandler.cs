using CodeGen.Commands.Abstract;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.Editing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CodeGen.Commands.Handlers.DocumentEditing
{
    public abstract class RoslynDocumentEditorCommandHandler<TCommand, TSyntaxNode> 
        : ICommandHandler<TCommand, CSharpTarget<TSyntaxNode>, TSyntaxNode, CSharpSyntaxNode, DocumentEditor>
        where TCommand : ICommand<TSyntaxNode, CSharpSyntaxNode, DocumentEditor>
        where TSyntaxNode : CSharpSyntaxNode
    {
        public CSharpTarget<TSyntaxNode> Target { get; set; }

        public abstract TCommand Command { get; internal set; }

        ITarget ICommandHandler.Target => Target;

        ICommand ICommandHandler.Command => Command;

        public abstract DocumentEditor ProcessDocument(DocumentEditor node);

        object ICommandHandler.ProcessDocument(object editor)
        {
            return ProcessDocument((DocumentEditor)editor);
        }
    }
}