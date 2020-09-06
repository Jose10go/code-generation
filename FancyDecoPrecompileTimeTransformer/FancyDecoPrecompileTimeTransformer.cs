using Microsoft.CodeAnalysis.CSharp.Syntax;
using System;
using Microsoft.CodeAnalysis;
using CodeGeneration.CSharp;
using static CodeGen.CSharp.Context.CSharpContext;
using System.Collections.Generic;
using System.Linq;
using static System.Linq.Enumerable;
using static System.String;
using CodeGen.CSharp;
using Microsoft.CodeAnalysis.CSharp;
using System.Text;

namespace FancyDecoTransformer
{
    class __DynamicType 
    {
        public __DynamicType(params object[] args)
        {
            
        }

        public void Decorate(object f) { }
    }

    public class FancyDecoPrecompileTimeTransformer : CodeGenerationPrecompileTimeTransformer
    {
        private ArgumentListSyntax GetArguments(AttributeData att)
        {
            var atribute = att.ApplicationSyntaxReference.GetSyntax() as AttributeSyntax;
            var args = atribute.ArgumentList?.Arguments??new SeparatedSyntaxList<AttributeArgumentSyntax>();
            return SyntaxFactory.ArgumentList(
                       new SeparatedSyntaxList<ArgumentSyntax>()
                       .AddRange(args.Select(x => x.Expression is CastExpressionSyntax cast &&
                                                  cast.Type is IdentifierNameSyntax id &&
                                                  id.Identifier.ToString() is "dynamic" ?
                                                  SyntaxFactory.Argument(cast.Expression) : SyntaxFactory.Argument(x.Expression))));
        }

        private IEnumerable<AttributeData> GetDecorators(IMethodSymbol methodSymbol)
        {
            return methodSymbol.GetAttributes()
                               .Where(x => IsFancyDecoratorAttributeDefinition(x.AttributeClass))
                               .Reverse();
        }

        public override void Transform()
        {
            var implemented = Engine.Select<MethodDeclarationSyntax, ClassDeclarationSyntax>()
                                    .Where(x => IsFancyDecoratedMethod(x.SemanticSymbol as IMethodSymbol));

            foreach (var item in implemented)
            {
                var isStatic = item.SemanticSymbol.IsStatic;
                var methodName = item.Node.Identifier.ToString();
                var decorators = GetDecorators(item.SemanticSymbol as IMethodSymbol);
                var returnType = ConstructReturnType(item.SemanticSymbol as IMethodSymbol);
                var code = new CodeContext().StartOrContinueWith($"()=>{{({returnType})_{methodName}}}");
                foreach (var decorator in decorators)
                    code = new CodeContext()
                        .InjectType(GetDecoratorFromAttribute(decorator))
                            .As<__DynamicType>()
                        .InjectCode(code.GetCode() as ExpressionSyntax, out var __code)
                            .As(nameof(__code))
                        .InjectCode(GetArguments(decorator), out var __arguments)
                            .As(nameof(__arguments))
                        .StartOrContinueWith(() => new __DynamicType(__arguments).Decorate(__code));

                if (isStatic)
                {
                    item.Parent.Execute((ICreateProperty cmd) =>
                        cmd.WithName("__decorated" + methodName)
                           .WithAttributes()
                           .MakePrivate()
                           .MakeStatic()
                           .Returns(returnType)
                           .InitializeWith(code));

                    var paramNames = item.Node.ParameterList.Parameters.Select(x => x.Identifier.ToString());
                    item.Execute((ICloneMethod cmd) =>
                        cmd.WithAttributes()
                           .WithBody(new CodeContext()
                                .InjectId("__decorated" + methodName, out dynamic __methodName)
                                    .As(nameof(__methodName))
                                .InjectCode(SyntaxFactory.ParseArgumentList("(" + Join(',', paramNames) + ")"), out var __args)
                                    .As(nameof(__args))
                                .StartOrContinueWith(() => __methodName(__args))));

                    item.Execute((IModifyMethod cmd) =>
                        cmd.WithName("_" + methodName)
                           .MakePrivate()
                           .MakeStatic()
                           .WithAttributes());
                }
            }
        }

        private string GetDecoratorFromAttribute(AttributeData decorator)
        {
            var attName = decorator.AttributeClass.Name;
            return attName.Substring(0,attName.Length - nameof(Attribute).Length);
        }

        private string ConstructReturnType(IMethodSymbol methodSymbol)
        {
            StringBuilder result = new StringBuilder();
            if (methodSymbol.ReturnsVoid) 
            {
                result.Append("Action");
                if (methodSymbol.TypeArguments.Length > 0)
                    result.Append("<")
                          .Append(Join(',', methodSymbol.Parameters.Select(x => x.Type.Name)))
                          .Append(">");
            }
            else
                result.Append("Func<")
                      .Append(Join(',', methodSymbol.Parameters.Select(x=>x.Type.Name).Append(methodSymbol.ReturnType.Name)))
                      .Append(">");
            return result.ToString();
        }

        private bool IsFancyDecoratedMethod(IMethodSymbol symbol)
        {
            return symbol.GetAttributes().Any(x=>IsFancyDecoratorAttributeDefinition(x.AttributeClass));
        }

        private bool IsFancyDecoratorAttributeDefinition(INamedTypeSymbol symbol)
        {
            return symbol.BaseType.Name == "DecoratorAttribute";
        }

    }

}
