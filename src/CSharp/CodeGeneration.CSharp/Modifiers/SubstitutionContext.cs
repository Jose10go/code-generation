using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.Collections.Generic;
using System.Linq;

namespace CodeGen.CSharp.Context
{
    public abstract partial class CSharpContext
    {
        public enum SubstitutionKind { This, Parameter, DynamicMember,Value }
        public class Substitution
        {
            public Substitution(string value,SubstitutionKind substitutionKind)
            {
                SubstitutionKind = substitutionKind;
                Value = value;
            }
            public SubstitutionKind SubstitutionKind { get; }
            public string Value { get; }
        }

        public class SubstitutionContext 
        {
            public ParenthesizedLambdaExpressionSyntax CodeContext { get; }
            public Dictionary<string,Substitution> Substitutions { get; }

            public SubstitutionContext(ParenthesizedLambdaExpressionSyntax parenthesizedLambdaExpressionSyntax,Dictionary<string,Substitution> substitutions)
            {
                CodeContext = parenthesizedLambdaExpressionSyntax;
                Substitutions = substitutions;
            }

            public CSharpSyntaxNode GetReplacedBody() 
            {
                var bodySyntax = CodeContext.Body;
                bodySyntax = bodySyntax.ReplaceNodes(bodySyntax.DescendantNodes()
                                                               .OfType<IdentifierNameSyntax>()
                                                               .Where(x => Substitutions.ContainsKey(x.Identifier.ToString())),
                                                    (node, _) =>
                                                    {
                                                        var id = node.Identifier.ToString();
                                                        if (Substitutions[id].SubstitutionKind is SubstitutionKind.This)
                                                            return SyntaxFactory.ThisExpression();
                                                        return SyntaxFactory.IdentifierName(Substitutions[id].Value);
                                                    });

                return bodySyntax;
            }

        }
    }
}
