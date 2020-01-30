using CodeGen.Attributes;
using CodeGen.Context;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System;
using System.Collections.Generic;

namespace CodeGen.CSharp.Context
{
    public abstract partial class CSharpContext<TProcessEntity> : CodeGenContext<Solution, CSharpSyntaxNode,ISymbol, TProcessEntity>
    {
        public interface IReplaceInvocation : ICommandBuilder<InvocationExpressionSyntax>,
                                              IWithNewArgument<IReplaceInvocation, InvocationExpressionSyntax>
        {
        }

        [CommandBuilder]
        public class ReplaceInvocationCommandBuilder : IReplaceInvocation
        {
            public ITarget<InvocationExpressionSyntax> Target { get ; set ; }
            public IList<(Func<InvocationExpressionSyntax, ArgumentSyntax>, int)> NewArguments { get ; set ; }
            public ReplaceInvocationCommandBuilder()
            {
                NewArguments = new List<(Func<InvocationExpressionSyntax, ArgumentSyntax>, int)>();
            }
        }

    }
}
