using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
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
			   return new CloneCommand<TSyntaxNode>(){ 
				NewName = NewName, };
			}

            public ITarget<TSyntaxNode> Target { get; set; }
            CodeGenTypelessContext.ITarget CodeGenTypelessContext.ICommandBuilder<CloneCommand<TSyntaxNode>>.Target { get => Target; set => Target=(ITarget<TSyntaxNode>)value; }
		}
	}
}

