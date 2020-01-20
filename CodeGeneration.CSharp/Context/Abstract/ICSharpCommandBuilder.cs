﻿using CodeGen.Context;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;

namespace CodeGen.CSharp.Context
{
    public partial class CSharpContext<TProcessEntity> : CodeGenContext<Solution, CSharpSyntaxNode, TProcessEntity>
    {
        public interface ICSharpCommandBuilder : ICommandBuilder
        {
        }
    }
}