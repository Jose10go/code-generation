using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.Editing;
using Microsoft.CodeAnalysis;
using System;
using CodeGen.Context;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CodeGen.CSharp.Context
{
    public abstract partial class CSharpContext : CodeGenContext<Project, CSharpSyntaxNode, CompilationUnitSyntax, ISymbol>
    {
        public abstract class CommandHandler<TCommand> : CSharpSyntaxVisitor,ICommandHandler<TCommand>
            where TCommand : Core.ICommand
        {
            public TCommand Command { get; }
            protected DocumentEditor DocumentEditor { get; private set; }
            protected Guid Id { get; private set; }
            protected CommandHandler(Core.ICommand command)
            {
                this.Command = (TCommand)command;
            }

            public ISingleTarget<TOutputNode> ProccessTarget<TSpecificCommand, TNode, TOutputNode>(ISingleTarget<TNode> target, ICodeGenerationEngine engine)
                where TSpecificCommand : ICommandOn<TNode>, ICommandResult<TOutputNode>, TCommand
                where TNode : CSharpSyntaxNode
                where TOutputNode : CSharpSyntaxNode
            {
                var doc = engine.CurrentProject.GetDocument(target.Node.SyntaxTree);
                this.DocumentEditor=DocumentEditor.CreateAsync(doc).Result;//TODO: make async???
                this.Id = Guid.NewGuid();
                target.Node.Accept(this);
                var result = new CSharpSingleTarget<TOutputNode>(engine,this.Id, doc.FilePath);
                var document = this.DocumentEditor.GetChangedDocument();
                engine.CurrentProject = document.Project;
                return result;
            }
        }
    }
}