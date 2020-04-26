using CodeGen.Attributes;
using CodeGen.Context;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Editing;
using System.Linq;

namespace CodeGen.CSharp.Context
{
    public abstract partial class CSharpContext : CodeGenContext<Project, CSharpSyntaxNode, CompilationUnitSyntax, ISymbol>
    {
        [CommandHandler]
        public class ClassCloneCommandHandler :CommandHandler<ICloneClass> 
        {
            public ClassCloneCommandHandler(ICloneClass command) : base(command)
            {
            }

            public override void VisitClassDeclaration(ClassDeclarationSyntax node)
            {
                var modifiers = new SyntaxTokenList();
                if (Command.Modifiers != default)
                    modifiers=modifiers.Add(Command.Modifiers);
                if (Command.Abstract != default)
                    modifiers = modifiers.Add(Command.Abstract);
                if (Command.Static != default)
                    modifiers = modifiers.Add(Command.Static);
                if (Command.Partial != default)
                    modifiers = modifiers.Add(Command.Partial);
                var separatedBaseTypes = new SeparatedSyntaxList<BaseTypeSyntax>();
                if (Command.InheritsType != null)
                    separatedBaseTypes=separatedBaseTypes.Add(SyntaxFactory.SimpleBaseType(SyntaxFactory.ParseTypeName(Command.InheritsType)));
                if (Command.ImplementedInterfaces != null)
                    separatedBaseTypes=separatedBaseTypes.AddRange(Command.ImplementedInterfaces.Select(name => SyntaxFactory.SimpleBaseType(SyntaxFactory.ParseTypeName(name))));

                var classNode = node.WithIdentifier(SyntaxFactory.Identifier(Command.Name))
                                  .WithAttributeLists(Command.Attributes)
                                  .WithModifiers(modifiers)
                                  .WithAdditionalAnnotations(new SyntaxAnnotation($"{Id}"));

                if (Command.GenericTypes !=null && Command.GenericTypes.Count > 0)
                    classNode = classNode.WithTypeParameterList(
                        SyntaxFactory.TypeParameterList(
                            new SeparatedSyntaxList<TypeParameterSyntax>().AddRange(
                                Command.GenericTypes.Keys.Select(x => SyntaxFactory.TypeParameter(x)))));

                if (separatedBaseTypes.Count > 0)
                    classNode = classNode.WithBaseList(SyntaxFactory.BaseList().WithTypes(separatedBaseTypes));

                if (Command.GenericTypes != null && Command.GenericTypes.Count > 0)
                    classNode = classNode.WithConstraintClauses(
                        new SyntaxList<TypeParameterConstraintClauseSyntax>(
                            Command.GenericTypes.Where(item => item.Value.Count > 0)
                                                .Select(item => SyntaxFactory.TypeParameterConstraintClause(item.Key)
                                                                           .WithConstraints(new SeparatedSyntaxList<TypeParameterConstraintSyntax>()
                                                                           .AddRange(item.Value.Select(x => SyntaxFactory.TypeConstraint(SyntaxFactory.ParseTypeName(x))))))));
                DocumentEditor.InsertAfter(node,classNode);
            }
        
        }
    }
}