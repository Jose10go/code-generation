﻿using CodeGen.Core;
using System;
using System.Collections.Generic;
namespace CodeGen.Context
{
    public abstract partial class CodeGenContext<TProject, TRootNode,TSemanticModel,TProcessEntity> 
    {
        public interface ITarget<TNode> : ITarget
        {
            Func<TSemanticModel,TNode, bool> WhereSelector{ get;set; }
            IEnumerable<TNode> Select(TRootNode root);
            new ICodeGenerationEngine CodeGenerationEngine { get { return (this as ITarget).CodeGenerationEngine as ICodeGenerationEngine; }set { (this as ITarget).CodeGenerationEngine = value; } }
        }
    }
}
