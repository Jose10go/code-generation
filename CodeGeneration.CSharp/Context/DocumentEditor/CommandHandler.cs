﻿using CodeGen.Attributes;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.Editing;

namespace CodeGen.CSharp.Context.DocumentEdit
{
    public partial class CSharpContextDocumentEditor : CSharpContext<DocumentEditor>
    {
        public abstract class CommandHandler<TCommandBuilder, TSyntaxNode> : ICommandHandler<TCommandBuilder,TSyntaxNode>
            where TCommandBuilder : ICommandBuilder<TSyntaxNode>
            where TSyntaxNode : CSharpSyntaxNode
        {
            public TCommandBuilder Command { get; }
            public CommandHandler(ICommandBuilder<TSyntaxNode> command)
            {
                this.Command = (TCommandBuilder)command;
            }

            public void ProcessDocument(DocumentEditor processEntity)
            {
                var syntaxTreeroot= (CSharpSyntaxNode)processEntity.GetChangedRoot();
                var semantic=processEntity.SemanticModel;
                var nodes = Command.Target.Select(syntaxTreeroot,(node)=>semantic.GetSymbolInfo(node).Symbol);
                foreach (var item in nodes)
                    ProccesNode(item,processEntity);
            }

            public abstract void ProccesNode(TSyntaxNode node,DocumentEditor documentEditor);
        }
    }
}