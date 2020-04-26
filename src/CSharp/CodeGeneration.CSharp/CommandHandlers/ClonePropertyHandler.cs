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
                                  .WithType(SyntaxFactory.ParseTypeName(Command.ReturnType??node.Type.ToString()));

                var getAccessor = node.AccessorList.Accessors.FirstOrDefault(x => x.IsKind(SyntaxKind.GetAccessorDeclaration));
                var setAccessor = node.AccessorList.Accessors.FirstOrDefault(x => x.IsKind(SyntaxKind.SetKeyword));
                
                var newGetAccessor = (Command.GetModifier, Command.GetStatements,getAccessor) switch
                {
                    ({Count:0}, null, _) =>getAccessor,
                    (_, null, null) => SyntaxFactory.AccessorDeclaration(SyntaxKind.GetAccessorDeclaration)
                                                    .WithModifiers(Command.GetModifier),
                    (_, null, _) => getAccessor.WithModifiers(Command.GetModifier),
                    ({Count:0}, _, null) => SyntaxFactory.AccessorDeclaration(SyntaxKind.GetAccessorDeclaration)
                                                         .WithBody(Command.GetStatements),
                    ({ Count: 0}, _, _) => getAccessor.WithBody(Command.GetStatements),
                    (_, _, null) => SyntaxFactory.AccessorDeclaration(SyntaxKind.GetAccessorDeclaration)
                                                 .WithModifiers(Command.GetModifier)
                                                 .WithBody(Command.GetStatements),
                    (_, _, _) => getAccessor.WithModifiers(Command.GetModifier)
                                            .WithBody(Command.GetStatements),
                };

                var newSetAccessor = (Command.SetModifier, Command.SetStatements,setAccessor) switch
                {
                    ({ Count: 0 }, null, _) => setAccessor,
                    (_, null, null) => SyntaxFactory.AccessorDeclaration(SyntaxKind.SetAccessorDeclaration)
                                                    .WithModifiers(Command.SetModifier),
                    (_, null, _) => setAccessor.WithModifiers(Command.SetModifier),
                    ({ Count: 0 }, _, null) => SyntaxFactory.AccessorDeclaration(SyntaxKind.SetAccessorDeclaration)
                                                            .WithBody(Command.SetStatements),
                    ({ Count: 0 }, _, _) => setAccessor.WithBody(Command.SetStatements),
                    (_, _, null) => SyntaxFactory.AccessorDeclaration(SyntaxKind.SetAccessorDeclaration)
                                                 .WithModifiers(Command.SetModifier)
                                                 .WithBody(Command.SetStatements),
                    (_, _, _) => setAccessor.WithModifiers(Command.SetModifier)
                                            .WithBody(Command.SetStatements),
                };

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