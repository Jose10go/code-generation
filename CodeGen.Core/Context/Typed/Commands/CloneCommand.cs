using CodeGen.Attributes;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeGen.Context
{
    // Typed Context - Commands
    public partial class CodeGenContext<TProject, TRootNode, TProcessEntity>:CodeGen.Context.CodeGenTypelessContext
    {
        public class CloneCommand<TSyntaxNode> : ICommand<TSyntaxNode>
        {
            [BuilderProp]
            public Func<TSyntaxNode, string> NewName { get; set; }

            public ITarget<TSyntaxNode> Target { get; set; }

            public ICommandHandler Handler { get; set; }
            ITarget ICommand.Target => Target;
        }
    }
}
