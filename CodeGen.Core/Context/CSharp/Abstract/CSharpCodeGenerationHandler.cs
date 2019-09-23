using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.Editing;
using System;

namespace CodeGen.Context.CSharp
{
    public partial class ICSharpContext<TProcessEntity> : CodeGenContext<Solution, CSharpSyntaxNode, TProcessEntity>
    {
        public abstract class RoslynDocumentEditorCommandHandler<TCommand, TSyntaxNode>:
            ICommandHandler<TCommand, CSharpTarget<TSyntaxNode>,CSharpSyntaxNode>
            where TCommand : ICommand<TSyntaxNode>
            where TSyntaxNode : CSharpSyntaxNode
        {
            public CSharpTarget<TSyntaxNode> Target { get; set; }

            public abstract TCommand Command { get; internal set; }

            ITarget ICommandHandler.Target => Target;

            ICommand ICommandHandler.Command => Command;

            public abstract DocumentEditor ProcessDocument(DocumentEditor node);

            public TProcessEntity ProcessDocument(TProcessEntity entity)
            {
                throw new NotImplementedException();
            }

            object ICommandHandler.ProcessDocument(object editor)
            {
                return ProcessDocument((DocumentEditor)editor);
            }
        }
    }
}