using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Editing;
using CodeGen.Attributes;
using Microsoft.CodeAnalysis;
using CodeGen.Context;
using System.Linq;
using static CodeGen.CSharp.Extensions;
namespace CodeGen.CSharp.Context
{
    public abstract partial class CSharpContext : CodeGenContext<Project, CSharpSyntaxNode, CompilationUnitSyntax, ISymbol>
    {
        [CommandHandler]
        public class ClonePropertyCommandHandler : CommandHandler<ICloneProperty>
        {
            public ClonePropertyCommandHandler(ICloneProperty command) :base(command)
            {
            }

            public override void VisitPropertyDeclaration(PropertyDeclarationSyntax node)
            {
                var modifiers = GetModifiers(Command.Modifiers,Command.Abstract,Command.Static);

                var newNode = node.WithIdentifier(SyntaxFactory.ParseToken(Command.Name))
                                  .WithAttributeLists(Command.Attributes)
                                  .WithModifiers(modifiers)
                                  .WithAdditionalAnnotations(new SyntaxAnnotation($"{Id}"))
                                  .WithType(Command.ReturnType??node.Type);

                var getAccessor = node.AccessorList.Accessors.FirstOrDefault(x => x.IsKind(SyntaxKind.GetAccessorDeclaration));
                var setAccessor = node.AccessorList.Accessors.FirstOrDefault(x => x.IsKind(SyntaxKind.SetKeyword));

                var newGetAccessor = getAccessor??SyntaxFactory
                                                 .AccessorDeclaration(SyntaxKind.GetAccessorDeclaration)
                                                 .WithModifiers(Command.GetModifier)
                                                 .WithExpressionBody(Command.GetExpression ?? getAccessor?.ExpressionBody)
                                                 .WithBody(Command.GetStatements??getAccessor?.Body);

                var newSetAccessor = setAccessor ?? SyntaxFactory
                                                 .AccessorDeclaration(SyntaxKind.SetAccessorDeclaration)
                                                 .WithModifiers(Command.SetModifier)
                                                 .WithExpressionBody(Command.SetExpression ?? setAccessor?.ExpressionBody)
                                                 .WithBody(Command.SetStatements ?? setAccessor?.Body);


                var Accesors = new SyntaxList<AccessorDeclarationSyntax>();
                if (newGetAccessor != null)
                    Accesors = Accesors.Add(newGetAccessor);
                if(newSetAccessor!=null)
                    Accesors = Accesors.Add(newSetAccessor);
                newNode = newNode.WithAccessorList(SyntaxFactory.AccessorList(Accesors));

                DocumentEditor.InsertAfter(node,newNode);
            }
        }
    }
}