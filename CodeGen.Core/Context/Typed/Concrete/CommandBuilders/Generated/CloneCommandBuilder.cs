using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Editing;
using System;

namespace CodeGen.Context
{
    // Typed Context - Commands
    public partial class CodeGenContext<TProject, TRootNode, TProcessEntity>
    {
		public class CloneCommandBuilder<TSyntaxNode> : ICommandBuilder<CloneCommand<TSyntaxNode>, TSyntaxNode>
        {
			public Func<TSyntaxNode, string> NewName { get; set; }

            public CloneCommandBuilder<TSyntaxNode> WithNewName(Func<TSyntaxNode, string> value)
			{
			   NewName = value;
			   return this;
			}

            public CloneCommand<TSyntaxNode> Build()
            {
                var handler = new CSharp.ICSharpContext<DocumentEditor>.MethodCloneCommandHandler();
                return new CloneCommand<TSyntaxNode>()
                {
                    NewName = NewName,
                    Target = Target,
                    Handler = handler
                };
            }

            public void Go(TProcessEntity processEntity)
            {
                var command = Build();
                var result=command.Handler.ProcessDocument(processEntity);
                Console.WriteLine(result.ToString());
            }

            public void Go(object processEntity)
            {
                Go((TProcessEntity)processEntity);
            }

            public ITarget<TSyntaxNode> Target { get; set; }
            CodeGenTypelessContext.ITarget CodeGenTypelessContext.ICommandBuilder<CloneCommand<TSyntaxNode>>.Target { get => Target; set => Target=(ITarget<TSyntaxNode>)value; }
		}
	}
}

