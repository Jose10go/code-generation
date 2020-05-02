using Microsoft.CodeAnalysis.CSharp.Syntax;
using System;
using Microsoft.CodeAnalysis;
using CodeGeneration.CSharp;
using CodeGen.Context;
using Microsoft.CodeAnalysis.CSharp;
using static CodeGen.CSharp.Context.CSharpContext;
using System.Collections.Generic;

namespace FancyDecoTransformer
{
    using static CodeGenContext<Project, CSharpSyntaxNode, CompilationUnitSyntax, ISymbol>;
    class T1 { }
    class T2 { }
    class T3 { }
    class T4 { }
    class T5 { }
    class T6 { }
    class T7 { }
    class T8 { }
    class TResult { }

    public class FancyDecoTransformer : CodeGenerationTransformer
    {
        public override void Transform()
        {
            var targets = Engine.Select<MethodDeclarationSyntax, ClassDeclarationSyntax>()
                                .Where(target => IsFancyDecorator(target.Parent.SemanticSymbol))
                                .Where(target => IsDecorateImplementation(target.SemanticSymbol))
                                .Using(x => x.Node.Body,out var bodyKey);


            foreach (var target in targets)
            {
                CreateDecoratorFor<Action>(target);
                CreateDecoratorFor<Action<T1>>(target);
                CreateDecoratorFor<Action<T1,T2>>(target);
                CreateDecoratorFor<Action<T1,T2,T3>>(target);
                CreateDecoratorFor<Action<T1,T2,T3,T4>>(target);
                CreateDecoratorFor<Action<T1,T2,T3,T4,T5>>(target);
                CreateDecoratorFor<Action<T1,T2,T3,T4,T5,T6>>(target);
                CreateDecoratorFor<Action<T1,T2,T3,T4,T5,T6,T7>>(target);
                CreateDecoratorFor<Action<T1,T2,T3,T4,T5,T6,T7,T8>>(target);
                CreateDecoratorFor<Func<TResult>>(target);
                CreateDecoratorFor<Func<T1,TResult>>(target);
                CreateDecoratorFor<Func<T1,T2,TResult>>(target);
                CreateDecoratorFor<Func<T1,T2,T3,TResult>>(target);
                CreateDecoratorFor<Func<T1,T2,T3,T4,TResult>>(target);
                CreateDecoratorFor<Func<T1,T2,T3,T4,T5,TResult>>(target);
                CreateDecoratorFor<Func<T1,T2,T3,T4,T5,T6,TResult>>(target);
                CreateDecoratorFor<Func<T1,T2,T3,T4,T5,T6,T7,TResult>>(target);
                CreateDecoratorFor<Func<T1,T2,T3,T4,T5,T6,T8,TResult>>(target);
            }

            void CreateDecoratorFor<T>(ISingleTarget<MethodDeclarationSyntax, ClassDeclarationSyntax> target)
                where T : Delegate
            {
                target.Execute((ICloneMethod cmd) => cmd.Returns<T>()
                                                        .Get(bodyKey, out var body)
                                                        .WithParameters<T>("f")
                                                        .WithBody(body));//TODO:Replacements
            }

            var implemented = Engine.Select<AttributeSyntax, MethodDeclarationSyntax, ClassDeclarationSyntax>()
                                    .Where(att => IsFancyDecorator(att.SemanticSymbol))
                                    .Using(x=>x.Parent.Node.Identifier.ToString(),out var methodNameKey)
                                    .Using(x=>x.Parent.Node.ReturnType.ToString(), out var returnTypeKey)
                                    .Using(x=>x.Parent.Node.ParameterList.Parameters.Count, out var cantParamsKey);

            foreach (var item in implemented)
            {
                item.Grandparent.Execute((ICreateProperty cmd) => cmd.Get(methodNameKey, out var methodName)
                                                                     .WithName("__decorated" + methodName)
                                                                     .MakePrivate()
                                                                     .MakeStatic()
                                                                     .WithGet((dynamic @this) => new Log().Decorate(@this.methodName))//TODO:Initializer and remove log...
                                                                     .Get(returnTypeKey, out var returnType)
                                                                     .Returns(returnType));
               
                var cantParams=item.Get(cantParamsKey);
                
                item.Parent.Execute((ICloneMethod cmd) => 
                    cantParams switch
                    {
                        0 => cmd.WithAttributes()
                                .Get(methodNameKey, out var methodName)
                                .WithBody((dynamic @this) => @this.methodName(),
                                    new Dictionary<string,string>{{nameof(methodName),"__decorated"+methodName}}),
                        1 => cmd.WithAttributes()
                                .Get(methodNameKey, out var methodName)
                                .WithBody((dynamic @this,dynamic arg1) => @this.methodName(arg1),
                                    new Dictionary<string, string> { { nameof(methodName), "__decorated" + methodName } }),
                        2 => cmd.WithAttributes()
                                .Get(methodNameKey, out var methodName)
                                .WithBody((dynamic @this,dynamic arg1,dynamic arg2) => @this.methodName(arg1,arg2),
                                    new Dictionary<string, string> { { nameof(methodName), "__decorated" + methodName } }),
                        3 => cmd.WithAttributes()
                                .Get(methodNameKey, out var methodName)
                                .WithBody((dynamic @this,dynamic arg1,dynamic arg2,dynamic arg3) => @this.methodName(arg1,arg2,arg3),
                                    new Dictionary<string, string> { { nameof(methodName), "__decorated" + methodName } }),
                        4 => cmd.WithAttributes()
                                .Get(methodNameKey, out var methodName)
                                .WithBody((dynamic @this,dynamic arg1,dynamic arg2,dynamic arg3, dynamic arg4) => @this.methodName(arg1,arg2,arg3,arg4),
                                    new Dictionary<string, string> { { nameof(methodName), "__decorated" + methodName } }),
                        5 => cmd.WithAttributes()
                                .Get(methodNameKey, out var methodName)
                                .WithBody((dynamic @this,dynamic arg1,dynamic arg2,dynamic arg3,dynamic arg4,dynamic arg5) => @this.methodName(arg1,arg2,arg3,arg4,arg5),
                                    new Dictionary<string, string> { { nameof(methodName), "__decorated" + methodName } }),
                        6 => cmd.WithAttributes()
                                .Get(methodNameKey, out var methodName)
                                .WithBody((dynamic @this,dynamic arg1,dynamic arg2,dynamic arg3,dynamic arg4,dynamic arg5,dynamic arg6) => @this.methodName(arg1,arg2,arg3,arg4,arg5,arg6),
                                    new Dictionary<string, string> { { nameof(methodName), "__decorated" + methodName } }),
                        7 => cmd.WithAttributes()
                                .Get(methodNameKey, out var methodName)
                                .WithBody((dynamic @this,dynamic arg1,dynamic arg2,dynamic arg3,dynamic arg4,dynamic arg5,dynamic arg6,dynamic arg7) => @this.methodName(arg1,arg2,arg3,arg4,arg5,arg6,arg7),
                                    new Dictionary<string, string> { { nameof(methodName), "__decorated" + methodName } }),
                        8 => cmd.WithAttributes()
                                .Get(methodNameKey, out var methodName)
                                .WithBody((dynamic @this,dynamic arg1,dynamic arg2,dynamic arg3,dynamic arg4,dynamic arg5,dynamic arg6,dynamic arg7,dynamic arg8) => @this.methodName(arg1,arg2,arg3,arg4,arg5,arg6,arg7,arg8),
                                    new Dictionary<string, string> { { nameof(methodName), "__decorated" + methodName } }),
                        _ => throw new Exception()//TODO: throw proper Exception 
                    });


                item.Parent.Execute((IModifyMethod cmd) => cmd.Get(methodNameKey, out var methodName)
                                                              .WithName("_" + methodName)
                                                              .MakePrivate()
                                                              .WithAttributes());
            }
        }

        
        private bool IsDecorateImplementation(ISymbol symbol)
        {
            var methodSymbol = symbol as IMethodSymbol;
            return methodSymbol.Name == "Decorate";
        }

        private bool IsFancyDecorator(ISymbol symbol) 
        {
            var classSymbol = symbol as INamedTypeSymbol;
            return classSymbol.BaseType.Name == "Decorator";
        }

    }

}
