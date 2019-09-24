using CodeGen.Attributes;
using System;

namespace CodeGen.Context
{
    // Typed Context - Commands
    public partial class CodeGenContext<TProject, TRootNode, TProcessEntity>
    {
        public class CloneCommand<TSyntaxNode> : ICommand<TSyntaxNode>
        {
            [BuilderProp]
            public Func<TSyntaxNode, string> NewName { get; set; }

            public ITarget<TSyntaxNode> Target { get; set; }

            public ICommandHandler<ICommand<TSyntaxNode>, ITarget<TSyntaxNode>,TSyntaxNode> Handler { get; set; }

            CodeGenTypelessContext.ITarget CodeGenTypelessContext.ICommand.Target => Target;

            CodeGenTypelessContext.ICommandHandler CodeGenTypelessContext.ICommand.Handler =>Handler;
        }
    }
}
