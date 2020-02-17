using CodeGen.Core;
using System;
using System.Collections.Generic;
namespace CodeGen.Context
{
    public abstract partial class CodeGenContext<TProject, TRootNode,TSemanticModel,TProcessEntity> 
    {
        public abstract class Target<TNode> : ITarget
            where TNode:TRootNode
        {
            protected Func<TSemanticModel,TNode, bool> WhereSelector{ get;set; }
            public ICodeGenerationEngine CodeGenerationEngine { get; protected set; }
            
            protected Target(ICodeGenerationEngine codeGenerationEngine)
            {
                this.CodeGenerationEngine = codeGenerationEngine;
                this.WhereSelector = (semantic,node) =>true;
            }

            public abstract IEnumerable<TNode> Select(TRootNode root, Func<TNode, TSemanticModel> semanticModel); 
            
            public Target<TNode> Where(Func<TSemanticModel, TNode, bool> filter)
            {
                this.WhereSelector = filter;
                return this;
            }

            public Target<TNode> Where(Func<TNode, bool> filter)
            {
                this.WhereSelector = (_, node) => filter(node);
                return this;
            }

            public TCommandBuilder Execute<TCommandBuilder>()
                where TCommandBuilder : ICommandBuilder<TNode>
            {
                var commandBuilder = Resolver.ResolveCommandBuilder<TCommandBuilder, TNode>();
                commandBuilder.Target = this;
                return commandBuilder;
            }

        }
    }
}
