﻿using CodeGen.Attributes;
using CodeGen.Context;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System;
using System.Collections.Generic;

namespace CodeGen.CSharp.Context
{
    public abstract partial class CSharpContext<TProcessEntity> : CodeGenContext<Project, CSharpSyntaxNode,ISymbol, TProcessEntity>
    {
        public interface IReplaceInvocation : ICommand<InvocationExpressionSyntax>,
                                              IWithNewArgument<IReplaceInvocation, InvocationExpressionSyntax>
        {
        }

        [CommandBuilder]
        public class ReplaceInvocationCommand : IReplaceInvocation
        {
            public Target<InvocationExpressionSyntax> Target { get ; set ; }
            public IList<(Func<InvocationExpressionSyntax, ArgumentSyntax>, int)> NewArguments { get ; set ; }
            public ReplaceInvocationCommand()
            {
                NewArguments = new List<(Func<InvocationExpressionSyntax, ArgumentSyntax>, int)>();
            }
        }

    }
}
