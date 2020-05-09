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

namespace FancyDecoTransformer
{
    class T1 { }
    class T2 { }
    class T3 { }
    class T4 { }
    class T5 { }
    class T6 { }
    class T7 { }
    class T8 { }
    class TResult { }
    
    class __DynamicType 
    {
        public __DynamicType(params object[] args)
        {

        }

        public void Decorate(object f) { }
    }

    public class FancyDecoTransformer : CodeGenerationTransformer
    {
        private void GenerateAttributes()
        {
            var decoratorsDefinitions = 
                Engine.Select<ClassDeclarationSyntax,NamespaceDeclarationSyntax>()
                      .Where(target => IsFancyDecoratorDefinition(target.SemanticSymbol as INamedTypeSymbol));
            
            foreach (var target in decoratorsDefinitions)
            {
                var att =
                target.Parent.Execute((ICreateClass cmd) =>
                    cmd.MakePublic()
                       .InheritsFrom<DecoratorAttribute>()
                       .WithName(target.Node.Identifier.ToString() + "Attribute"));
                       
                       //Generate TypeCheck Attributes
                       //.WithAttributes(
                       // (target.SemanticSymbol as ITypeSymbol).BaseType.Name switch
                       // {
                       //     "Decorator" => new string[0],
                       //     _ => throw new Exception()
                       // }));

                foreach (var constructor in att.AsMultiple().Select<ConstructorDeclarationSyntax>())
                    constructor.Execute((ICloneConstructor cmd) => 
                        cmd.WithBody(new CodeContext())
                           .On(att.Node));
            }
        }

        private void GenerateOnDecoratedMethods()
        {
            var implemented = Engine.Select<MethodDeclarationSyntax, ClassDeclarationSyntax>()
                                    .Where(x => IsFancyDecoratedMethod(x.SemanticSymbol as IMethodSymbol));

            foreach (var item in implemented)
            {
                var methodName = item.Node.Identifier.ToString();
                var decorators = GetDecorators(item.SemanticSymbol as IMethodSymbol);
                var returnType = item.Node.ReturnType.ToString();
                item.Parent.Execute((ICreateProperty cmd) =>
                {
                    cmd.WithName("__decorated" + methodName)
                       .MakePrivate()
                       .MakeStatic()
                       .Returns(returnType);
                    var code = new CodeContext().StartOrContinueWith($"({returnType}){methodName}");
                    foreach (var decorator in decorators)
                        code = new CodeContext()
                                 .InjectType(decorator.AttributeClass.Name)
                                    .As<__DynamicType>()
                                 .InjectCode(code.GetCode() as ExpressionSyntax,out var __code)
                                    .As(nameof(__code))
                                 .InjectCode(GetArguments(decorator),out var __arguments)
                                    .As(nameof(__arguments))
                                 .StartOrContinueWith(()=> new __DynamicType(__arguments).Decorate(__code));
                    return cmd.WithGetBody(code);
                });

                var paramNames = item.Node.ParameterList.Parameters.Select(x => x.Identifier.ToString());
                item.Execute((ICloneMethod cmd) =>
                    cmd.WithAttributes()
                       .WithBody(new CodeContext()
                                        .InjectId("__decorated" + methodName, out dynamic __methodName)
                                            .As(nameof(__methodName))
                                        .InjectCode(SyntaxFactory.ParseArgumentList("(" + Join(',', paramNames) + ")"), out var __args)
                                            .As(nameof(__args))
                                        .StartOrContinueWith(()=>__methodName(__args))));
               
                item.Execute((IModifyMethod cmd) => 
                    cmd.WithName("_" + methodName)
                       .MakePrivate()
                       .WithAttributes());
            }
        }
        
        private ArgumentListSyntax GetArguments(AttributeData att)
        {
            return SyntaxFactory.ArgumentList(
                       new SeparatedSyntaxList<ArgumentSyntax>().AddRange(
                           (att.ApplicationSyntaxReference
                               .GetSyntax() as AttributeSyntax)
                               .ArgumentList
                               .Arguments.Select(x => x.Expression is CastExpressionSyntax cast &&
                                                    cast.Type is IdentifierNameSyntax id &&
                                                    id.Identifier.ToString() is "dynamic" ?
                                                    SyntaxFactory.Argument(cast.Expression) : SyntaxFactory.Argument(x.Expression))));
        }

        private IEnumerable<AttributeData> GetDecorators(IMethodSymbol methodSymbol)
        {
            return methodSymbol.GetAttributes()
                               .Where(x => IsFancyDecoratorDefinition(x.AttributeClass))
                               .Reverse();
        }

        public override void Transform()
        {
            GenerateAttributes();
            GenerateOnDecoratedMethods();
        }

        private bool IsFancyDecoratedMethod(IMethodSymbol symbol)
        {
            return symbol.GetAttributes().Any(x=>IsFancyDecoratorAttributeDefinition(x.AttributeClass));
        }

        private bool IsFancyDecoratorAttributeDefinition(INamedTypeSymbol symbol)
        {
            return symbol.BaseType.Name == "DecoratorAttribute";
        }

        private bool IsFancyDecoratorDefinition(INamedTypeSymbol symbol) 
        {
            return symbol.BaseType.Name == "Decorator";
        }

    }

}
