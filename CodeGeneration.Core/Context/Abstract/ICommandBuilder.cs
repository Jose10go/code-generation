﻿using CodeGen.Attributes;
using System.Linq;

namespace CodeGen.Context
{
    public partial class CodeGenContext<TProject, TRootNode, TSemanticModel, TProcessEntity>
    {
        public interface ICommandBuilder<TSyntaxNode> : Core.ICommandBuilder
            where TSyntaxNode:TRootNode
        {
            ITarget<TSyntaxNode> Target { get; set; }

            void Go()
            {
                 var handler=Resolver.ResolveCommandHandler(this);
                 Target.CodeGenerationEngine.ApplyChanges(handler);
            }

        }
    }
}