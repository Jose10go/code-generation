using CodeGen.Attributes;
using CodeGen.Context;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System;
using System.Linq;
using static CodeGen.CSharp.Extensions;

namespace CodeGen.CSharp.Context
{
    public abstract partial class CSharpContext : CodeGenContext<Project, CSharpSyntaxNode,CompilationUnitSyntax, ISymbol>
    {
        [CommandModifier]
        public interface IWithParameters<TCommand>
            where TCommand:Core.ICommand
        {
            ParameterListSyntax Parameters { get; set; }

            public TCommand WithRawParameters(params string[] parameters) 
            {
                if (Parameters is null)
                    Parameters = SyntaxFactory.ParameterList();
                var parameterList = string.Join(',', parameters);
                Parameters=Parameters.AddParameters(SyntaxFactory.ParseParameterList(parameterList).Parameters.ToArray());
                return (TCommand)this;
            }

            public TCommand WithParameters<T>(string parameterName)
            {
                return WithRawParameters(GetCSharpName<T>()+" "+parameterName);
            }

            public TCommand WithParameters<T1,T2>(string parameterName1,string parameterName2)
            {
                return WithRawParameters(GetCSharpName<T1>() + " " + parameterName1,
                                         GetCSharpName<T2>() + " " + parameterName2);
            }

            public TCommand WithParameters<T1, T2, T3>(string parameterName1, string parameterName2, string parameterName3)
            {
                return WithRawParameters(GetCSharpName<T1>() + " " + parameterName1,
                                         GetCSharpName<T2>() + " " + parameterName2,
                                         GetCSharpName<T3>() + " " + parameterName3);
            }

            public TCommand WithParameters<T1, T2, T3,T4>(string parameterName1, string parameterName2, string parameterName3, string parameterName4)
            {
                return WithRawParameters(GetCSharpName<T1>() + " " + parameterName1,
                                         GetCSharpName<T2>() + " " + parameterName2,
                                         GetCSharpName<T3>() + " " + parameterName3,
                                         GetCSharpName<T4>() + " " + parameterName4);
            }

            public TCommand WithParameters<T1, T2, T3, T4, T5>(string parameterName1, string parameterName2, string parameterName3, string parameterName4,string parameterName5)
            {
                return WithRawParameters(GetCSharpName<T1>() + " " + parameterName1,
                                         GetCSharpName<T2>() + " " + parameterName2,
                                         GetCSharpName<T3>() + " " + parameterName3,
                                         GetCSharpName<T3>() + " " + parameterName4,
                                         GetCSharpName<T4>() + " " + parameterName5);
            }

            public TCommand WithAtributedParameter(string parameterName, params string[] atributes)
            {
                var parameter = Parameters.Parameters.First(x => x.Identifier.ToString() == parameterName);
                Parameters = Parameters.ReplaceNode(parameter, 
                    parameter.AddAttributeLists(atributes.Select(
                        x=>SyntaxFactory.AttributeList().AddAttributes(
                            SyntaxFactory.Attribute(
                                SyntaxFactory.ParseName(x))))
                    .ToArray()));

                return (TCommand)this;
            }

            public TCommand WithAtributedParameter<T>(string parameterName)
                where T:Attribute
            {
                return WithAtributedParameter(parameterName,GetCSharpName<T>());
            }

            public TCommand WithAtributedParameter<T1,T2>(string parameterName)
               where T1 : Attribute
               where T2 : Attribute
            {
                return WithAtributedParameter(parameterName, GetCSharpName<T1>(),
                                                             GetCSharpName<T2>());
            }

            public TCommand WithAtributedParameter<T1, T2,T3>(string parameterName)
               where T1 : Attribute
               where T2 : Attribute
               where T3 : Attribute
            {
                return WithAtributedParameter(parameterName, GetCSharpName<T1>(),
                                                             GetCSharpName<T2>(),
                                                             GetCSharpName<T3>());
            }

            public TCommand WithAtributedParameter<T1, T2, T3, T4>(string parameterName)
              where T1 : Attribute
              where T2 : Attribute
              where T3 : Attribute
              where T4 : Attribute
            {
                return WithAtributedParameter(parameterName, GetCSharpName<T1>(),
                                                             GetCSharpName<T2>(),
                                                             GetCSharpName<T3>(),
                                                             GetCSharpName<T4>());
            }

            public TCommand WithAtributedParameter<T1, T2, T3, T4,T5>(string parameterName)
                where T1 : Attribute
                where T2 : Attribute
                where T3 : Attribute
                where T4 : Attribute
                where T5 : Attribute
            {
                return WithAtributedParameter(parameterName, GetCSharpName<T1>(),
                                                             GetCSharpName<T2>(),
                                                             GetCSharpName<T3>(),
                                                             GetCSharpName<T4>(),
                                                             GetCSharpName<T5>());
            }

            public TCommand WithOutParameters(params string[] parameterNames) 
            {
                foreach (var item in parameterNames)
                {
                    var parameter = Parameters.Parameters.First(x => x.Identifier.ToString() == item);
                    Parameters = Parameters.ReplaceNode(parameter,parameter.AddModifiers(SyntaxFactory.Token(SyntaxKind.OutKeyword)));
                }
                return (TCommand)this;
            }

            public TCommand WithRefParameter(params string[] parameterNames)
            {
                foreach (var item in parameterNames)
                {
                    var parameter = Parameters.Parameters.First(x => x.Identifier.ToString() == item);
                    Parameters = Parameters.ReplaceNode(parameter, parameter.AddModifiers(SyntaxFactory.Token(SyntaxKind.RefKeyword)));
                }
                return (TCommand)this;
            }

            public TCommand WithThisParameter(string parameterName)
            {
                var parameter = Parameters.Parameters.First(x => x.Identifier.ToString() == parameterName);
                Parameters = Parameters.ReplaceNode(parameter, parameter.AddModifiers(SyntaxFactory.Token(SyntaxKind.ThisKeyword)));
                return (TCommand)this;
            }

            public TCommand WithDefaultValue(string parameterName,string value)
            {
                var parameter = Parameters.Parameters.First(x => x.Identifier.ToString() == parameterName);
                Parameters = Parameters.ReplaceNode(parameter, 
                    parameter.WithDefault(
                        SyntaxFactory.EqualsValueClause(
                            SyntaxFactory.LiteralExpression(SyntaxKind.StringLiteralExpression,
                                SyntaxFactory.Literal(value)))));
                return (TCommand)this;
            }

            public TCommand WithDefaultValue(string parameterName, bool value)
            {
                var parameter = Parameters.Parameters.First(x => x.Identifier.ToString() == parameterName);
                Parameters = Parameters.ReplaceNode(parameter,
                    parameter.WithDefault(
                        SyntaxFactory.EqualsValueClause(
                            SyntaxFactory.LiteralExpression(value?SyntaxKind.TrueLiteralExpression:SyntaxKind.FalseLiteralExpression))));
                return (TCommand)this;
            }

            public TCommand WithDefaultValue(string parameterName, int value)
            {
                var parameter = Parameters.Parameters.First(x => x.Identifier.ToString() == parameterName);
                Parameters = Parameters.ReplaceNode(parameter,
                    parameter.WithDefault(
                        SyntaxFactory.EqualsValueClause(
                            SyntaxFactory.LiteralExpression(SyntaxKind.NumericLiteralToken,
                                SyntaxFactory.Literal(value)))));
                return (TCommand)this;
            }

            public TCommand WithDefaultValue(string parameterName, float value)
            {
                var parameter = Parameters.Parameters.First(x => x.Identifier.ToString() == parameterName);
                Parameters = Parameters.ReplaceNode(parameter,
                    parameter.WithDefault(
                        SyntaxFactory.EqualsValueClause(
                            SyntaxFactory.LiteralExpression(SyntaxKind.NumericLiteralToken,
                                SyntaxFactory.Literal(value)))));
                return (TCommand)this;
            }

            public TCommand WithDefaultValue(string parameterName, double value)
            {
                var parameter = Parameters.Parameters.First(x => x.Identifier.ToString() == parameterName);
                Parameters = Parameters.ReplaceNode(parameter,
                    parameter.WithDefault(
                        SyntaxFactory.EqualsValueClause(
                            SyntaxFactory.LiteralExpression(SyntaxKind.NumericLiteralToken,
                                SyntaxFactory.Literal(value)))));
                return (TCommand)this;
            }

            public TCommand WithDefaultValue(string parameterName, decimal value)
            {
                var parameter = Parameters.Parameters.First(x => x.Identifier.ToString() == parameterName);
                Parameters = Parameters.ReplaceNode(parameter,
                    parameter.WithDefault(
                        SyntaxFactory.EqualsValueClause(
                            SyntaxFactory.LiteralExpression(SyntaxKind.NumericLiteralToken,
                                SyntaxFactory.Literal(value)))));
                return (TCommand)this;
            }
        }
    }
}
