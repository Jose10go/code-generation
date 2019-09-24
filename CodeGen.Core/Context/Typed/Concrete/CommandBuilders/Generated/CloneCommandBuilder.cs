using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using System;

namespace CodeGen.Context
{
    // Typed Context - Commands
    public partial class CodeGenContext<TProject, TRootNode, TProcessEntity>:CodeGen.Context.CodeGenTypelessContext
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
		}
	}
}

