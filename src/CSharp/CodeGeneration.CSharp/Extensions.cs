using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System;
using System.Collections.Generic;
using System.Linq;
using static CodeGen.CSharp.Context.CSharpContext;

namespace CodeGen.CSharp
{
    internal static class Extensions
    {
        public static SyntaxTokenList GetModifiers(params SyntaxToken[] tokens)
        {
            SyntaxTokenList result = new SyntaxTokenList();
            foreach (var item in tokens)
                if (item != default)
                    result = result.Add(item);
            return result;
        }

        public static SyntaxTokenList GetModifiers(SyntaxTokenList list, params SyntaxToken[] tokens)
        {
            SyntaxTokenList result = list == default ? new SyntaxTokenList() : list;
            foreach (var item in tokens)
                if (item != default)
                    result = result.Add(item);
            return result;
        }

        public static BaseListSyntax GetBaseTypes(BaseListSyntax implementedInterfaces, BaseTypeSyntax inheritsType = null)
        {
            return (implementedInterfaces, inheritsType) switch
            {
                (null, null) => null,
                (_, null) => implementedInterfaces,
                (null, _) => SyntaxFactory.BaseList().AddTypes(inheritsType),
                (_, _) => SyntaxFactory.BaseList().AddTypes(inheritsType)
                                                  .AddTypes(implementedInterfaces.Types.ToArray())

            };
        }

        public static string GetCSharpName<T>()
        {
            return GetCSharpName(typeof(T));
        }

        public static string GetCSharpName(Type type)
        {
            //TODO: Complete al primitive types an correctly generate arrays and nested types...
            if (type == typeof(object))
                return "object";
            if (type == typeof(string))
                return "string";
            if (type == typeof(bool))
                return "bool";
            if (type == typeof(byte))
                return "byte";
            if (type == typeof(short))
                return "short";
            if (type == typeof(ushort))
                return "ushort";
            if (type == typeof(int))
                return "int";
            if (type == typeof(uint))
                return "uint";
            if (type == typeof(long))
                return "long";
            if (type == typeof(ulong))
                return "ulong";
            if (type == typeof(float))
                return "float";
            if (type == typeof(double))
                return "double";
            if (type == typeof(decimal))
                return "decimal";
            if (type.GetGenericArguments().Length == 0)
                return type.Name;
            var genericArguments = type.GetGenericArguments();
            var typeName = type.Name.Substring(0, type.Name.IndexOf('`'));
            return typeName + "<" + String.Join(',', genericArguments.Select(GetCSharpName)) + ">";
        }

    }

    internal class SelectSyntaxRewriter<TNode0, TNode1,TNode2> : SelectorSyntaxRewriter<TNode2>
      where TNode0 : CSharpSyntaxNode
      where TNode1 : CSharpSyntaxNode
      where TNode2 : CSharpSyntaxNode
    {
        public IEnumerable<CSharpSingleTarget<TNode0, TNode1,TNode2>> Result { get; private set; }
        public SelectSyntaxRewriter(CSharpCodeGenerationEngine engine, string docPath)
            : base(engine, docPath)
        {
            Result = Enumerable.Empty<CSharpSingleTarget<TNode0, TNode1,TNode2>>();
        }

        protected override T Mark<T>(T node, out bool shouldContinue)
        {
            shouldContinue = true;
            var selector = new SelectSyntaxRewriter<TNode0,TNode1>(engine, docPath);
            var newNode = selector.VisitRoot(node,false);
            if (selector.Result.FirstOrDefault() != null)
            {
                var grandparent = new CSharpSingleTarget<TNode2>(engine, Guid.NewGuid(), docPath);
                Result = Result.Concat(selector.Result.Select(x => new CSharpSingleTarget<TNode0, TNode1,TNode2>(engine, x.Id, new CSharpSingleTarget<TNode1, TNode2>(engine,x.Parent.Id,grandparent, docPath),docPath)));
                return (T)newNode.WithAdditionalAnnotations(new SyntaxAnnotation($"{grandparent.Id}"));
            }
            shouldContinue = false;
            return node;
        }
    }

    internal class SelectSyntaxRewriter<TNode0, TNode1> : SelectorSyntaxRewriter<TNode1>
      where TNode0 : CSharpSyntaxNode
      where TNode1 : CSharpSyntaxNode
    {
        public IEnumerable<CSharpSingleTarget<TNode0, TNode1>> Result { get;private set;}
        public SelectSyntaxRewriter(CSharpCodeGenerationEngine engine, string docPath)
            : base(engine,docPath)
        {
            Result = Enumerable.Empty<CSharpSingleTarget<TNode0, TNode1>>();
        }

        protected override T Mark<T>(T node,out bool shouldContinue)
        {
            shouldContinue = true;
            var selector = new SelectSyntaxRewriter<TNode0>(engine, docPath);
            var newNode = selector.VisitRoot(node,false);
            if (selector.Result.FirstOrDefault() != null)
            {
                var parent = new CSharpSingleTarget<TNode1>(engine, Guid.NewGuid(), docPath);
                Result=Result.Concat(selector.Result.Select(x => new CSharpSingleTarget<TNode0, TNode1>(engine, x.Id,parent, docPath)));
                return (T)newNode.WithAdditionalAnnotations(new SyntaxAnnotation($"{parent.Id}"));
            }
            shouldContinue = false;
            return node;
        }
    }

    internal class SelectSyntaxRewriter<TNode0> : SelectorSyntaxRewriter<TNode0>
      where TNode0 : CSharpSyntaxNode
    {
        public IEnumerable<CSharpSingleTarget<TNode0>> Result { get; private set; }
        internal SelectSyntaxRewriter(CSharpCodeGenerationEngine engine, string docPath)
            : base(engine, docPath)
        {
            Result = Enumerable.Empty<CSharpSingleTarget<TNode0>>();
        }

        protected override T Mark<T>(T node, out bool shouldContinue)
        {
            shouldContinue = true;
            var target = new CSharpSingleTarget<TNode0>(engine, Guid.NewGuid(), docPath);
            Result = Result.Append(target);
            return node.WithAdditionalAnnotations(new SyntaxAnnotation($"{target.Id}"));
        }
    }

    internal abstract class SelectorSyntaxRewriter<TNode>: CSharpSyntaxRewriter
        where TNode:CSharpSyntaxNode
    {
        protected readonly CSharpCodeGenerationEngine engine;
        protected readonly string docPath;
        protected abstract T Mark<T>(T node, out bool shouldContinue)
            where T : CSharpSyntaxNode;
        
        private CSharpSyntaxNode root;
        private bool allowRootSelection;
        
        public CSharpSyntaxNode VisitRoot(CSharpSyntaxNode root,bool allowRootSelection=true)
        {
            this.root = root;
            this.allowRootSelection = allowRootSelection;
            return root.Accept(this) as CSharpSyntaxNode;
        }

        protected SelectorSyntaxRewriter(CSharpCodeGenerationEngine engine, string docPath)
        {
            this.engine = engine;
            this.docPath = docPath;
        }

        public override SyntaxNode VisitIdentifierName(IdentifierNameSyntax node)
        {
            bool shouldContinue = true;
            if (node is TNode && (node!=root || allowRootSelection))
                node = Mark(node, out shouldContinue); 
            if (shouldContinue)
                return base.VisitIdentifierName(node);
            else return node;
        }

        public override SyntaxNode VisitQualifiedName(QualifiedNameSyntax node)
        {
            bool shouldContinue = true;
            if (node is TNode && (node!=root || allowRootSelection))
                node = Mark(node, out shouldContinue);
            if (shouldContinue)
                return base.VisitQualifiedName(node);
            else return node;
        }

        public override SyntaxNode VisitGenericName(GenericNameSyntax node)
        {
            bool shouldContinue = true;
            if (node is TNode && (node!=root || allowRootSelection))
                node = Mark(node, out shouldContinue) ; 
            if (shouldContinue)
                return base.VisitGenericName(node);
            else return node;
        }

        public override SyntaxNode VisitTypeArgumentList(TypeArgumentListSyntax node)
        {
            bool shouldContinue = true;
            if (node is TNode && (node!=root || allowRootSelection))
                node = Mark(node, out shouldContinue);
            if (shouldContinue)
                return base.VisitTypeArgumentList(node);
            else return node;
        }

        public override SyntaxNode VisitAliasQualifiedName(AliasQualifiedNameSyntax node)
        {
            bool shouldContinue = true;
            if (node is TNode && (node!=root || allowRootSelection))
                node = Mark(node, out shouldContinue);
            if (shouldContinue)
                return base.VisitAliasQualifiedName(node);
            else return node;
        }

        public override SyntaxNode VisitPredefinedType(PredefinedTypeSyntax node)
        {
            bool shouldContinue = true;
            if (node is TNode && (node!=root || allowRootSelection))
                node = Mark(node, out shouldContinue);
            if (shouldContinue)
                return base.VisitPredefinedType(node);
            else
                return node;
        }

        public override SyntaxNode VisitArrayType(ArrayTypeSyntax node)
        {
            bool shouldContinue = true;
            if (node is TNode && (node!=root || allowRootSelection)) 
                node = Mark(node, out shouldContinue);
            if (shouldContinue)
                return base.VisitArrayType(node);
            else return node;
        }

        public override SyntaxNode VisitArrayRankSpecifier(ArrayRankSpecifierSyntax node)
        {
            bool shouldContinue = true; 
            if (node is TNode && (node!=root || allowRootSelection)) 
                node = Mark(node, out shouldContinue); 
            if (shouldContinue)
                return base.VisitArrayRankSpecifier(node);
            else return node;
        }

        public override SyntaxNode VisitPointerType(PointerTypeSyntax node)
        {
            bool shouldContinue = true; 
            if (node is TNode && (node!=root || allowRootSelection)) 
                node = Mark(node, out shouldContinue);
            if (shouldContinue)
                return base.VisitPointerType(node);
            else return node;
        }

        public override SyntaxNode VisitNullableType(NullableTypeSyntax node)
        {
            bool shouldContinue = true; 
            if (node is TNode && (node!=root || allowRootSelection))
                node = Mark(node, out shouldContinue); 
            if (shouldContinue)
                return base.VisitNullableType(node);
            else return node;
        }

        public override SyntaxNode VisitTupleType(TupleTypeSyntax node)
        {
            bool shouldContinue = true; 
            if (node is TNode && (node!=root || allowRootSelection))
                node = Mark(node, out shouldContinue);
            if (shouldContinue)
                return base.VisitTupleType(node);
            else return node;
        }

        public override SyntaxNode VisitTupleElement(TupleElementSyntax node)
        {
            bool shouldContinue = true; 
            if (node is TNode && (node!=root || allowRootSelection)) 
                node = Mark(node, out shouldContinue);
            if (shouldContinue)
                return base.VisitTupleElement(node);
            else return node;
        }

        public override SyntaxNode VisitOmittedTypeArgument(OmittedTypeArgumentSyntax node)
        {
            bool shouldContinue = true; 
            if (node is TNode && (node!=root || allowRootSelection))
                node = Mark(node, out shouldContinue);
            if (shouldContinue)
                return base.VisitOmittedTypeArgument(node);
            else return node;
        }

        public override SyntaxNode VisitRefType(RefTypeSyntax node)
        {
            bool shouldContinue = true; 
            if (node is TNode && (node!=root || allowRootSelection)) 
                node = Mark(node, out shouldContinue); 
            if (shouldContinue)
                return base.VisitRefType(node);
            else return node;
        }

        public override SyntaxNode VisitParenthesizedExpression(ParenthesizedExpressionSyntax node)
        {
            bool shouldContinue = true; 
            if (node is TNode && (node!=root || allowRootSelection)) 
                node = Mark(node, out shouldContinue);
            if (shouldContinue)
                return base.VisitParenthesizedExpression(node);
            else return node;
        }

        public override SyntaxNode VisitTupleExpression(TupleExpressionSyntax node)
        {
            bool shouldContinue = true; 
            if (node is TNode && (node!=root || allowRootSelection))
                node = Mark(node, out shouldContinue);
            if (shouldContinue)
                return base.VisitTupleExpression(node);
            else return node;
        }

        public override SyntaxNode VisitPrefixUnaryExpression(PrefixUnaryExpressionSyntax node)
        {
            bool shouldContinue = true;
            if (node is TNode && (node!=root || allowRootSelection)) 
                node = Mark(node, out shouldContinue);
            if (shouldContinue)
                return base.VisitPrefixUnaryExpression(node);
            else return node;
        }

        public override SyntaxNode VisitAwaitExpression(AwaitExpressionSyntax node)
        {
            bool shouldContinue = true;
            if (node is TNode && (node!=root || allowRootSelection)) 
                node = Mark(node, out shouldContinue);
            if (shouldContinue)
                return base.VisitAwaitExpression(node);
            else return node;
        }

        public override SyntaxNode VisitPostfixUnaryExpression(PostfixUnaryExpressionSyntax node)
        {
            bool shouldContinue = true;
            if (node is TNode && (node!=root || allowRootSelection)) 
                node = Mark(node, out shouldContinue);
            if (shouldContinue)
                return base.VisitPostfixUnaryExpression(node);
            else return node;
        }

        public override SyntaxNode VisitMemberAccessExpression(MemberAccessExpressionSyntax node)
        {
            bool shouldContinue = true;
            if (node is TNode && (node!=root || allowRootSelection)) 
                node = Mark(node, out shouldContinue);
            if (shouldContinue)
                return base.VisitMemberAccessExpression(node);
            else return node;
        }

        public override SyntaxNode VisitConditionalAccessExpression(ConditionalAccessExpressionSyntax node)
        {
            bool shouldContinue = true;
            if (node is TNode && (node!=root || allowRootSelection))
                node = Mark(node, out shouldContinue);
            if (shouldContinue)
                return base.VisitConditionalAccessExpression(node);
            else return node;
        }

        public override SyntaxNode VisitMemberBindingExpression(MemberBindingExpressionSyntax node)
        {
            bool shouldContinue = true;
            if (node is TNode && (node!=root || allowRootSelection))
                node = Mark(node, out shouldContinue);
            if (shouldContinue)
                return base.VisitMemberBindingExpression(node);
            else return node;
        }

        public override SyntaxNode VisitElementBindingExpression(ElementBindingExpressionSyntax node)
        {
            bool shouldContinue = true; 
            if (node is TNode && (node!=root || allowRootSelection)) 
                node = Mark(node, out shouldContinue); 
            if (shouldContinue)
                return base.VisitElementBindingExpression(node);
            else return node;
        }

        public override SyntaxNode VisitRangeExpression(RangeExpressionSyntax node)
        {
            bool shouldContinue = true; 
            if (node is TNode && (node!=root || allowRootSelection))
                node = Mark(node, out shouldContinue); 
            if (shouldContinue)
                return base.VisitRangeExpression(node);
            else return node;
        }

        public override SyntaxNode VisitImplicitElementAccess(ImplicitElementAccessSyntax node)
        {
            bool shouldContinue = true; 
            if (node is TNode && (node!=root || allowRootSelection)) 
                node = Mark(node, out shouldContinue);
            if (shouldContinue)
                return base.VisitImplicitElementAccess(node);
            else return node;
        }

        public override SyntaxNode VisitBinaryExpression(BinaryExpressionSyntax node)
        {
            bool shouldContinue = true; 
            if (node is TNode && (node!=root || allowRootSelection)) 
                node = Mark(node, out shouldContinue);
            if (shouldContinue)
                return base.VisitBinaryExpression(node);
            else return node;
        }

        public override SyntaxNode VisitAssignmentExpression(AssignmentExpressionSyntax node)
        {
            bool shouldContinue = true;
            if (node is TNode && (node!=root || allowRootSelection)) 
                node = Mark(node, out shouldContinue); 
            if (shouldContinue)
                return base.VisitAssignmentExpression(node);
            else return node;
        }

        public override SyntaxNode VisitConditionalExpression(ConditionalExpressionSyntax node)
        {
            bool shouldContinue = true;
            if (node is TNode && (node!=root || allowRootSelection)) 
                node = Mark(node, out shouldContinue);
            if (shouldContinue)
                return base.VisitConditionalExpression(node);
            else return node;
        }

        public override SyntaxNode VisitThisExpression(ThisExpressionSyntax node)
        {
            bool shouldContinue = true; 
            if (node is TNode && (node!=root || allowRootSelection))
                node = Mark(node, out shouldContinue);
            if (shouldContinue)
                return base.VisitThisExpression(node);
            else return node;
        }

        public override SyntaxNode VisitBaseExpression(BaseExpressionSyntax node)
        {
            bool shouldContinue = true;
            if (node is TNode && (node!=root || allowRootSelection)) 
                node = Mark(node, out shouldContinue); 
            if (shouldContinue)
                return base.VisitBaseExpression(node);
            else return node;
        }

        public override SyntaxNode VisitLiteralExpression(LiteralExpressionSyntax node)
        {
            bool shouldContinue = true; 
            if (node is TNode && (node!=root || allowRootSelection))
                node = Mark(node, out shouldContinue);
            if (shouldContinue)
                return base.VisitLiteralExpression(node);
            else return node;
        }

        public override SyntaxNode VisitMakeRefExpression(MakeRefExpressionSyntax node)
        {
            bool shouldContinue = true; 
            if (node is TNode && (node!=root || allowRootSelection)) 
                node = Mark(node, out shouldContinue);
            if (shouldContinue)
                return base.VisitMakeRefExpression(node);
            else return node;
        }

        public override SyntaxNode VisitRefTypeExpression(RefTypeExpressionSyntax node)
        {
            bool shouldContinue = true; 
            if (node is TNode && (node!=root || allowRootSelection)) 
                node = Mark(node, out shouldContinue); 
            if (shouldContinue)
                return base.VisitRefTypeExpression(node);
            else return node;
        }

        public override SyntaxNode VisitRefValueExpression(RefValueExpressionSyntax node)
        {
            bool shouldContinue = true; 
            if (node is TNode && (node!=root || allowRootSelection)) 
                node = Mark(node, out shouldContinue);
            if (shouldContinue)
                return base.VisitRefValueExpression(node);
            else return node;
        }

        public override SyntaxNode VisitCheckedExpression(CheckedExpressionSyntax node)
        {
            bool shouldContinue = true; 
            if (node is TNode && (node!=root || allowRootSelection)) 
                node = Mark(node, out shouldContinue);
            if (shouldContinue)
                return base.VisitCheckedExpression(node);
            else return node;
        }

        public override SyntaxNode VisitDefaultExpression(DefaultExpressionSyntax node)
        {
            bool shouldContinue = true; 
            if (node is TNode && (node!=root || allowRootSelection)) 
                node = Mark(node, out shouldContinue);
            if (shouldContinue)
                return base.VisitDefaultExpression(node);
            else return node;
        }

        public override SyntaxNode VisitTypeOfExpression(TypeOfExpressionSyntax node)
        {
            bool shouldContinue = true; 
            if (node is TNode && (node!=root || allowRootSelection)) 
                node = Mark(node, out shouldContinue);
            if (shouldContinue)
                return base.VisitTypeOfExpression(node);
            else return node;
        }

        public override SyntaxNode VisitSizeOfExpression(SizeOfExpressionSyntax node)
        {
            bool shouldContinue = true;
            if (node is TNode && (node!=root || allowRootSelection)) 
                node = Mark(node, out shouldContinue); 
            if (shouldContinue)
                return base.VisitSizeOfExpression(node);
            else return node;
        }

        public override SyntaxNode VisitInvocationExpression(InvocationExpressionSyntax node)
        {
            bool shouldContinue = true; 
            if (node is TNode && (node!=root || allowRootSelection)) 
                node = Mark(node, out shouldContinue);
            if (shouldContinue)
                return base.VisitInvocationExpression(node);
            else return node;
        }

        public override SyntaxNode VisitElementAccessExpression(ElementAccessExpressionSyntax node)
        {
            bool shouldContinue = true; 
            if (node is TNode && (node!=root || allowRootSelection)) 
                node = Mark(node, out shouldContinue);
            if (shouldContinue)
                return base.VisitElementAccessExpression(node);
            else return node;
        }

        public override SyntaxNode VisitArgumentList(ArgumentListSyntax node)
        {
            bool shouldContinue = true; 
            if (node is TNode && (node!=root || allowRootSelection)) 
                node = Mark(node, out shouldContinue);
            if (shouldContinue)
                return base.VisitArgumentList(node);
            else return node;
        }

        public override SyntaxNode VisitBracketedArgumentList(BracketedArgumentListSyntax node)
        {
            bool shouldContinue = true; 
            if (node is TNode && (node!=root || allowRootSelection)) 
                node = Mark(node, out shouldContinue);
            if (shouldContinue)
                return base.VisitBracketedArgumentList(node);
            else return node;
        }

        public override SyntaxNode VisitArgument(ArgumentSyntax node)
        {
            bool shouldContinue = true; 
            if (node is TNode && (node!=root || allowRootSelection)) 
                node = Mark(node, out shouldContinue); 
            if (shouldContinue)
                return base.VisitArgument(node);
            else return node;
        }

        public override SyntaxNode VisitNameColon(NameColonSyntax node)
        {
            bool shouldContinue = true; 
            if (node is TNode && (node!=root || allowRootSelection)) 
                node = Mark(node, out shouldContinue);
            if (shouldContinue)
                return base.VisitNameColon(node);
            else return node;
        }

        public override SyntaxNode VisitDeclarationExpression(DeclarationExpressionSyntax node)
        {
            bool shouldContinue = true; 
            if (node is TNode && (node!=root || allowRootSelection)) 
                node = Mark(node, out shouldContinue);
            if (shouldContinue)
                return base.VisitDeclarationExpression(node);
            else return node;
        }

        public override SyntaxNode VisitCastExpression(CastExpressionSyntax node)
        {
            bool shouldContinue = true;
            if (node is TNode && (node!=root || allowRootSelection)) 
                node = Mark(node, out shouldContinue); 
            if (shouldContinue)
                return base.VisitCastExpression(node);
            else return node;
        }

        public override SyntaxNode VisitAnonymousMethodExpression(AnonymousMethodExpressionSyntax node)
        {
            bool shouldContinue = true;
            if (node is TNode && (node!=root || allowRootSelection))
                node = Mark(node, out shouldContinue);
            if (shouldContinue)
                return base.VisitAnonymousMethodExpression(node);
            else return node;
        }

        public override SyntaxNode VisitSimpleLambdaExpression(SimpleLambdaExpressionSyntax node)
        {
            bool shouldContinue = true; 
            if (node is TNode && (node!=root || allowRootSelection)) 
                node = Mark(node, out shouldContinue); 
            if (shouldContinue)
                return base.VisitSimpleLambdaExpression(node);
            else return node;
        }

        public override SyntaxNode VisitRefExpression(RefExpressionSyntax node)
        {
            bool shouldContinue = true;
            if (node is TNode && (node!=root || allowRootSelection)) 
                node = Mark(node, out shouldContinue);
            if (shouldContinue)
                return base.VisitRefExpression(node);
            else return node;
        }

        public override SyntaxNode VisitParenthesizedLambdaExpression(ParenthesizedLambdaExpressionSyntax node)
        {
            bool shouldContinue = true; 
            if (node is TNode && (node!=root || allowRootSelection)) 
                node = Mark(node, out shouldContinue);
            if (shouldContinue)
                return base.VisitParenthesizedLambdaExpression(node);
            else return node;
        }

        public override SyntaxNode VisitInitializerExpression(InitializerExpressionSyntax node)
        {
            bool shouldContinue = true;
            if (node is TNode && (node!=root || allowRootSelection)) 
                node = Mark(node, out shouldContinue);
            if (shouldContinue)
                return base.VisitInitializerExpression(node);
            else return node;
        }

        public override SyntaxNode VisitObjectCreationExpression(ObjectCreationExpressionSyntax node)
        {
            bool shouldContinue = true; 
            if (node is TNode && (node!=root || allowRootSelection)) 
                node = Mark(node, out shouldContinue);
            if (shouldContinue)
                return base.VisitObjectCreationExpression(node);
            else return node;
        }

        public override SyntaxNode VisitAnonymousObjectMemberDeclarator(AnonymousObjectMemberDeclaratorSyntax node)
        {
            bool shouldContinue = true; 
            if (node is TNode && (node!=root || allowRootSelection)) 
                node = Mark(node, out shouldContinue);
            if (shouldContinue)
                return base.VisitAnonymousObjectMemberDeclarator(node);
            else return node;
        }

        public override SyntaxNode VisitAnonymousObjectCreationExpression(AnonymousObjectCreationExpressionSyntax node)
        {
            bool shouldContinue = true; 
            if (node is TNode && (node!=root || allowRootSelection)) 
                node = Mark(node, out shouldContinue);
            if (shouldContinue)
                return base.VisitAnonymousObjectCreationExpression(node);
            else return node;
        }

        public override SyntaxNode VisitArrayCreationExpression(ArrayCreationExpressionSyntax node)
        {
            bool shouldContinue = true; 
            if (node is TNode && (node!=root || allowRootSelection)) 
                node = Mark(node, out shouldContinue); 
            if (shouldContinue)
                return base.VisitArrayCreationExpression(node);
            else return node;
        }

        public override SyntaxNode VisitImplicitArrayCreationExpression(ImplicitArrayCreationExpressionSyntax node)
        {
            bool shouldContinue = true; 
            if (node is TNode && (node!=root || allowRootSelection)) 
                node = Mark(node, out shouldContinue); 
            if (shouldContinue)
                return base.VisitImplicitArrayCreationExpression(node);
            else return node;
        }

        public override SyntaxNode VisitStackAllocArrayCreationExpression(StackAllocArrayCreationExpressionSyntax node)
        {
            bool shouldContinue = true; 
            if (node is TNode && (node!=root || allowRootSelection)) 
                node = Mark(node, out shouldContinue); 
            if (shouldContinue)
                return base.VisitStackAllocArrayCreationExpression(node);
            else return node;
        }

        public override SyntaxNode VisitImplicitStackAllocArrayCreationExpression(ImplicitStackAllocArrayCreationExpressionSyntax node)
        {
            bool shouldContinue = true; 
            if (node is TNode && (node!=root || allowRootSelection)) 
                node = Mark(node, out shouldContinue);
            if (shouldContinue)
                return base.VisitImplicitStackAllocArrayCreationExpression(node);
            else return node;
        }

        public override SyntaxNode VisitQueryExpression(QueryExpressionSyntax node)
        {
            bool shouldContinue = true;
            if (node is TNode && (node!=root || allowRootSelection))
                node = Mark(node, out shouldContinue);
            if (shouldContinue)
                return base.VisitQueryExpression(node);
            else return node;
        }

        public override SyntaxNode VisitQueryBody(QueryBodySyntax node)
        {
            bool shouldContinue = true;
            if (node is TNode && (node!=root || allowRootSelection)) 
                node = Mark(node, out shouldContinue);
            if (shouldContinue)
                return base.VisitQueryBody(node);
            else return node;
        }

        public override SyntaxNode VisitFromClause(FromClauseSyntax node)
        {
            bool shouldContinue = true; 
            if (node is TNode && (node!=root || allowRootSelection)) 
                node = Mark(node, out shouldContinue);
            if (shouldContinue)
                return base.VisitFromClause(node);
            else return node;
        }

        public override SyntaxNode VisitLetClause(LetClauseSyntax node)
        {
            bool shouldContinue = true; 
            if (node is TNode && (node!=root || allowRootSelection)) 
                node = Mark(node, out shouldContinue);
            if (shouldContinue)
                return base.VisitLetClause(node);
            else return node;
        }

        public override SyntaxNode VisitJoinClause(JoinClauseSyntax node)
        {
            bool shouldContinue = true; 
            if (node is TNode && (node!=root || allowRootSelection)) 
                node = Mark(node, out shouldContinue);
            if (shouldContinue)
                return base.VisitJoinClause(node);
            else return node;
        }

        public override SyntaxNode VisitJoinIntoClause(JoinIntoClauseSyntax node)
        {
            bool shouldContinue = true; 
            if (node is TNode && (node!=root || allowRootSelection)) 
                node = Mark(node, out shouldContinue);
            if (shouldContinue)
                return base.VisitJoinIntoClause(node);
            else return node;
        }

        public override SyntaxNode VisitWhereClause(WhereClauseSyntax node)
        {
            bool shouldContinue = true; 
            if (node is TNode && (node!=root || allowRootSelection)) 
                node = Mark(node, out shouldContinue);
            if (shouldContinue)
                return base.VisitWhereClause(node);
            else return node;
        }

        public override SyntaxNode VisitOrderByClause(OrderByClauseSyntax node)
        {
            bool shouldContinue = true; 
            if (node is TNode && (node!=root || allowRootSelection)) 
                node = Mark(node, out shouldContinue);
            if (shouldContinue)
                return base.VisitOrderByClause(node);
            else return node;
        }

        public override SyntaxNode VisitOrdering(OrderingSyntax node)
        {
            bool shouldContinue = true; 
            if (node is TNode && (node!=root || allowRootSelection)) 
                node = Mark(node, out shouldContinue);
            if (shouldContinue)
                return base.VisitOrdering(node);
            else return node;
        }

        public override SyntaxNode VisitSelectClause(SelectClauseSyntax node)
        {
            bool shouldContinue = true;
            if (node is TNode && (node!=root || allowRootSelection))
                node = Mark(node, out shouldContinue); 
            if (shouldContinue)
                return base.VisitSelectClause(node);
            else return node;
        }

        public override SyntaxNode VisitGroupClause(GroupClauseSyntax node)
        {
            bool shouldContinue = true;
            if (node is TNode && (node!=root || allowRootSelection)) 
                node = Mark(node, out shouldContinue); 
            if (shouldContinue)
                return base.VisitGroupClause(node);
            else return node;
        }

        public override SyntaxNode VisitQueryContinuation(QueryContinuationSyntax node)
        {
            bool shouldContinue = true;
            if (node is TNode && (node!=root || allowRootSelection)) 
                node = Mark(node, out shouldContinue); 
            if (shouldContinue)
                return base.VisitQueryContinuation(node);
            else return node;
        }

        public override SyntaxNode VisitOmittedArraySizeExpression(OmittedArraySizeExpressionSyntax node)
        {
            bool shouldContinue = true; 
            if (node is TNode && (node!=root || allowRootSelection)) 
                node = Mark(node, out shouldContinue); 
            if (shouldContinue)
                return base.VisitOmittedArraySizeExpression(node);
            else return node;
        }

        public override SyntaxNode VisitInterpolatedStringExpression(InterpolatedStringExpressionSyntax node)
        {
            bool shouldContinue = true;
            if (node is TNode && (node!=root || allowRootSelection)) 
                node = Mark(node, out shouldContinue); 
            if (shouldContinue)
                return base.VisitInterpolatedStringExpression(node);
            else return node;
        }

        public override SyntaxNode VisitIsPatternExpression(IsPatternExpressionSyntax node)
        {
            bool shouldContinue = true;
            if (node is TNode && (node!=root || allowRootSelection)) 
                node = Mark(node, out shouldContinue); 
            if (shouldContinue)
                return base.VisitIsPatternExpression(node);
            else return node;
        }

        public override SyntaxNode VisitThrowExpression(ThrowExpressionSyntax node)
        {
            bool shouldContinue = true;
            if (node is TNode && (node!=root || allowRootSelection))
                node = Mark(node, out shouldContinue);
            if (shouldContinue)
                return base.VisitThrowExpression(node);
            else return node;
        }

        public override SyntaxNode VisitWhenClause(WhenClauseSyntax node)
        {
            bool shouldContinue = true;
            if (node is TNode && (node!=root || allowRootSelection)) 
                node = Mark(node, out shouldContinue);
            if (shouldContinue)
                return base.VisitWhenClause(node);
            else return node;
        }

        public override SyntaxNode VisitDiscardPattern(DiscardPatternSyntax node)
        {
            bool shouldContinue = true;
            if (node is TNode && (node!=root || allowRootSelection)) 
                node = Mark(node, out shouldContinue); 
            if (shouldContinue)
                return base.VisitDiscardPattern(node);
            else return node;
        }

        public override SyntaxNode VisitDeclarationPattern(DeclarationPatternSyntax node)
        {
            bool shouldContinue = true; 
            if (node is TNode && (node!=root || allowRootSelection)) 
                node = Mark(node, out shouldContinue);
            if (shouldContinue)
                return base.VisitDeclarationPattern(node);
            else return node;
        }

        public override SyntaxNode VisitVarPattern(VarPatternSyntax node)
        {
            bool shouldContinue = true; 
            if (node is TNode && (node!=root || allowRootSelection))
                node = Mark(node, out shouldContinue);
            if (shouldContinue)
                return base.VisitVarPattern(node);
            else return node;
        }

        public override SyntaxNode VisitRecursivePattern(RecursivePatternSyntax node)
        {
            bool shouldContinue = true; 
            if (node is TNode && (node!=root || allowRootSelection)) 
                node = Mark(node, out shouldContinue);
            if (shouldContinue)
                return base.VisitRecursivePattern(node);
            else return node;
        }

        public override SyntaxNode VisitPositionalPatternClause(PositionalPatternClauseSyntax node)
        {
            bool shouldContinue = true;
            if (node is TNode && (node!=root || allowRootSelection))
                node = Mark(node, out shouldContinue); 
            if (shouldContinue)
                return base.VisitPositionalPatternClause(node);
            else return node;
        }

        public override SyntaxNode VisitPropertyPatternClause(PropertyPatternClauseSyntax node)
        {
            bool shouldContinue = true; 
            if (node is TNode && (node!=root || allowRootSelection))
                node = Mark(node, out shouldContinue);
            if (shouldContinue)
                return base.VisitPropertyPatternClause(node);
            else return node;
        }

        public override SyntaxNode VisitSubpattern(SubpatternSyntax node)
        {
            bool shouldContinue = true;
            if (node is TNode && (node!=root || allowRootSelection)) 
                node = Mark(node, out shouldContinue); 
            if (shouldContinue)
                return base.VisitSubpattern(node);
            else return node;
        }

        public override SyntaxNode VisitConstantPattern(ConstantPatternSyntax node)
        {
            bool shouldContinue = true;
            if (node is TNode && (node!=root || allowRootSelection)) 
                node = Mark(node, out shouldContinue);
            if (shouldContinue)
                return base.VisitConstantPattern(node);
            else return node;
        }

        public override SyntaxNode VisitInterpolatedStringText(InterpolatedStringTextSyntax node)
        {
            bool shouldContinue = true;
            if (node is TNode && (node!=root || allowRootSelection)) 
                node = Mark(node, out shouldContinue);
            if (shouldContinue)
                return base.VisitInterpolatedStringText(node);
            else return node;
        }

        public override SyntaxNode VisitInterpolation(InterpolationSyntax node)
        {
            bool shouldContinue = true; 
            if (node is TNode && (node!=root || allowRootSelection)) 
                node = Mark(node, out shouldContinue); 
            if (shouldContinue)
                return base.VisitInterpolation(node);
            else return node;
        }

        public override SyntaxNode VisitInterpolationAlignmentClause(InterpolationAlignmentClauseSyntax node)
        {
            bool shouldContinue = true; 
            if (node is TNode && (node!=root || allowRootSelection))
                node = Mark(node, out shouldContinue);
            if (shouldContinue)
                return base.VisitInterpolationAlignmentClause(node);
            else return node;
        }

        public override SyntaxNode VisitInterpolationFormatClause(InterpolationFormatClauseSyntax node)
        {
            bool shouldContinue = true;
            if (node is TNode && (node!=root || allowRootSelection)) 
                node = Mark(node, out shouldContinue);
            if (shouldContinue)
                return base.VisitInterpolationFormatClause(node);
            else return node;
        }

        public override SyntaxNode VisitGlobalStatement(GlobalStatementSyntax node)
        {
            bool shouldContinue = true;
            if (node is TNode && (node!=root || allowRootSelection)) 
                node = Mark(node, out shouldContinue); 
            if (shouldContinue)
                return base.VisitGlobalStatement(node);
            else return node;
        }

        public override SyntaxNode VisitBlock(BlockSyntax node)
        {
            bool shouldContinue = true; 
            if (node is TNode && (node!=root || allowRootSelection))
                node = Mark(node, out shouldContinue); 
            if (shouldContinue)
                return base.VisitBlock(node);
            else return node;
        }

        public override SyntaxNode VisitLocalFunctionStatement(LocalFunctionStatementSyntax node)
        {
            bool shouldContinue = true; 
            if (node is TNode && (node!=root || allowRootSelection)) 
                node = Mark(node, out shouldContinue); 
            if (shouldContinue)
                return base.VisitLocalFunctionStatement(node);
            else return node;
        }

        public override SyntaxNode VisitLocalDeclarationStatement(LocalDeclarationStatementSyntax node)
        {
            bool shouldContinue = true; 
            if (node is TNode && (node!=root || allowRootSelection)) 
                node = Mark(node, out shouldContinue); 
            if (shouldContinue)
                return base.VisitLocalDeclarationStatement(node);
            else return node;
        }

        public override SyntaxNode VisitVariableDeclaration(VariableDeclarationSyntax node)
        {
            bool shouldContinue = true;
            if (node is TNode && (node!=root || allowRootSelection)) 
                node = Mark(node, out shouldContinue);
            if (shouldContinue)
                return base.VisitVariableDeclaration(node);
            else return node;
        }

        public override SyntaxNode VisitVariableDeclarator(VariableDeclaratorSyntax node)
        {
            bool shouldContinue = true;
            if (node is TNode && (node!=root || allowRootSelection))
                node = Mark(node, out shouldContinue);
            if (shouldContinue)
                return base.VisitVariableDeclarator(node);
            else return node;
        }

        public override SyntaxNode VisitEqualsValueClause(EqualsValueClauseSyntax node)
        {
            bool shouldContinue = true; 
            if (node is TNode && (node!=root || allowRootSelection)) 
                node = Mark(node, out shouldContinue);
            if (shouldContinue)
                return base.VisitEqualsValueClause(node);
            else return node;
        }

        public override SyntaxNode VisitSingleVariableDesignation(SingleVariableDesignationSyntax node)
        {
            bool shouldContinue = true; 
            if (node is TNode && (node!=root || allowRootSelection)) 
                node = Mark(node, out shouldContinue);
            if (shouldContinue)
                return base.VisitSingleVariableDesignation(node);
            else return node;
        }

        public override SyntaxNode VisitDiscardDesignation(DiscardDesignationSyntax node)
        {
            bool shouldContinue = true;
            if (node is TNode && (node!=root || allowRootSelection)) 
                node = Mark(node, out shouldContinue);
            if (shouldContinue)
                return base.VisitDiscardDesignation(node);
            else return node;
        }

        public override SyntaxNode VisitParenthesizedVariableDesignation(ParenthesizedVariableDesignationSyntax node)
        {
            bool shouldContinue = true; 
            if (node is TNode && (node!=root || allowRootSelection)) 
                node = Mark(node, out shouldContinue);
            if (shouldContinue)
                return base.VisitParenthesizedVariableDesignation(node);
            else return node;
        }

        public override SyntaxNode VisitExpressionStatement(ExpressionStatementSyntax node)
        {
            bool shouldContinue = true; 
            if (node is TNode && (node!=root || allowRootSelection)) 
                node = Mark(node, out shouldContinue);
            if (shouldContinue)
                return base.VisitExpressionStatement(node);
            else return node;
        }

        public override SyntaxNode VisitEmptyStatement(EmptyStatementSyntax node)
        {
            bool shouldContinue = true; 
            if (node is TNode && (node!=root || allowRootSelection)) 
                node = Mark(node, out shouldContinue); 
            if (shouldContinue)
                return base.VisitEmptyStatement(node);
            else return node;
        }

        public override SyntaxNode VisitLabeledStatement(LabeledStatementSyntax node)
        {
            bool shouldContinue = true;
            if (node is TNode && (node!=root || allowRootSelection)) 
                node = Mark(node, out shouldContinue); 
            if (shouldContinue)
                return base.VisitLabeledStatement(node);
            else return node;
        }

        public override SyntaxNode VisitGotoStatement(GotoStatementSyntax node)
        {
            bool shouldContinue = true;
            if (node is TNode && (node!=root || allowRootSelection))
                node = Mark(node, out shouldContinue);
            if (shouldContinue)
                return base.VisitGotoStatement(node);
            else return node;
        }

        public override SyntaxNode VisitBreakStatement(BreakStatementSyntax node)
        {
            bool shouldContinue = true; 
            if (node is TNode && (node!=root || allowRootSelection))
                node = Mark(node, out shouldContinue); 
            if (shouldContinue)
                return base.VisitBreakStatement(node);
            else return node;
        }

        public override SyntaxNode VisitContinueStatement(ContinueStatementSyntax node)
        {
            bool shouldContinue = true; 
            if (node is TNode && (node!=root || allowRootSelection)) 
                node = Mark(node, out shouldContinue); 
            if (shouldContinue)
                return base.VisitContinueStatement(node);
            else return node;
        }

        public override SyntaxNode VisitReturnStatement(ReturnStatementSyntax node)
        {
            bool shouldContinue = true; 
            if (node is TNode && (node!=root || allowRootSelection)) 
                node = Mark(node, out shouldContinue); 
            if (shouldContinue)
                return base.VisitReturnStatement(node);
            else return node;
        }

        public override SyntaxNode VisitThrowStatement(ThrowStatementSyntax node)
        {
            bool shouldContinue = true;
            if (node is TNode && (node!=root || allowRootSelection)) 
                node = Mark(node, out shouldContinue);
            if (shouldContinue)
                return base.VisitThrowStatement(node);
            else return node;
        }

        public override SyntaxNode VisitYieldStatement(YieldStatementSyntax node)
        {
            bool shouldContinue = true;
            if (node is TNode && (node!=root || allowRootSelection)) 
                node = Mark(node, out shouldContinue);
            if (shouldContinue)
                return base.VisitYieldStatement(node);
            else return node;
        }

        public override SyntaxNode VisitWhileStatement(WhileStatementSyntax node)
        {
            bool shouldContinue = true; 
            if (node is TNode && (node!=root || allowRootSelection)) 
                node = Mark(node, out shouldContinue);
            if (shouldContinue)
                return base.VisitWhileStatement(node);
            else return node;
        }

        public override SyntaxNode VisitDoStatement(DoStatementSyntax node)
        {
            bool shouldContinue = true; 
            if (node is TNode && (node!=root || allowRootSelection)) 
                node = Mark(node, out shouldContinue);
            if (shouldContinue)
                return base.VisitDoStatement(node);
            else return node;
        }

        public override SyntaxNode VisitForStatement(ForStatementSyntax node)
        {
            bool shouldContinue = true; 
            if (node is TNode && (node!=root || allowRootSelection)) 
                node = Mark(node, out shouldContinue);
            if (shouldContinue)
                return base.VisitForStatement(node);
            else return node;
        }

        public override SyntaxNode VisitForEachStatement(ForEachStatementSyntax node)
        {
            bool shouldContinue = true; 
            if (node is TNode && (node!=root || allowRootSelection)) 
                node = Mark(node, out shouldContinue);
            if (shouldContinue)
                return base.VisitForEachStatement(node);
            else return node;
        }

        public override SyntaxNode VisitForEachVariableStatement(ForEachVariableStatementSyntax node)
        {
            bool shouldContinue = true; 
            if (node is TNode && (node!=root || allowRootSelection)) 
                node = Mark(node, out shouldContinue);
            if (shouldContinue)
                return base.VisitForEachVariableStatement(node);
            else return node;
        }

        public override SyntaxNode VisitUsingStatement(UsingStatementSyntax node)
        {
            bool shouldContinue = true; 
            if (node is TNode && (node!=root || allowRootSelection)) 
                node = Mark(node, out shouldContinue);
            if (shouldContinue)
                return base.VisitUsingStatement(node);
            else return node;
        }

        public override SyntaxNode VisitFixedStatement(FixedStatementSyntax node)
        {
            bool shouldContinue = true; 
            if (node is TNode && (node!=root || allowRootSelection)) 
                node = Mark(node, out shouldContinue); 
            if (shouldContinue)
                return base.VisitFixedStatement(node);
            else return node;
        }

        public override SyntaxNode VisitCheckedStatement(CheckedStatementSyntax node)
        {
            bool shouldContinue = true; 
            if (node is TNode && (node!=root || allowRootSelection)) 
                node = Mark(node, out shouldContinue);
            if (shouldContinue)
                return base.VisitCheckedStatement(node);
            else return node;
        }

        public override SyntaxNode VisitUnsafeStatement(UnsafeStatementSyntax node)
        {
            bool shouldContinue = true; 
            if (node is TNode && (node!=root || allowRootSelection))
                node = Mark(node, out shouldContinue); 
            if (shouldContinue)
                return base.VisitUnsafeStatement(node);
            else return node;
        }

        public override SyntaxNode VisitLockStatement(LockStatementSyntax node)
        {
            bool shouldContinue = true; 
            if (node is TNode && (node!=root || allowRootSelection)) 
                node = Mark(node, out shouldContinue);
            if (shouldContinue)
                return base.VisitLockStatement(node);
            else return node;
        }

        public override SyntaxNode VisitIfStatement(IfStatementSyntax node)
        {
            bool shouldContinue = true;
            if (node is TNode && (node!=root || allowRootSelection))
                node = Mark(node, out shouldContinue);
            if (shouldContinue)
                return base.VisitIfStatement(node);
            else return node;
        }

        public override SyntaxNode VisitElseClause(ElseClauseSyntax node)
        {
            bool shouldContinue = true;
            if (node is TNode && (node!=root || allowRootSelection))
                node = Mark(node, out shouldContinue);
            if (shouldContinue)
                return base.VisitElseClause(node);
            else return node;
        }

        public override SyntaxNode VisitSwitchStatement(SwitchStatementSyntax node)
        {
            bool shouldContinue = true;
            if (node is TNode && (node!=root || allowRootSelection)) 
                node = Mark(node, out shouldContinue);
            if (shouldContinue)
                return base.VisitSwitchStatement(node);
            else return node;
        }

        public override SyntaxNode VisitSwitchSection(SwitchSectionSyntax node)
        {
            bool shouldContinue = true; 
            if (node is TNode && (node!=root || allowRootSelection))
                node = Mark(node, out shouldContinue); 
            if (shouldContinue)
                return base.VisitSwitchSection(node);
            else return node;
        }

        public override SyntaxNode VisitCasePatternSwitchLabel(CasePatternSwitchLabelSyntax node)
        {
            bool shouldContinue = true;
            if (node is TNode && (node!=root || allowRootSelection))
                node = Mark(node, out shouldContinue); 
            if (shouldContinue)
                return base.VisitCasePatternSwitchLabel(node);
            else return node;
        }

        public override SyntaxNode VisitCaseSwitchLabel(CaseSwitchLabelSyntax node)
        {
            bool shouldContinue = true; 
            if (node is TNode && (node!=root || allowRootSelection))
                node = Mark(node, out shouldContinue);
            if (shouldContinue)
                return base.VisitCaseSwitchLabel(node);
            else return node;
        }

        public override SyntaxNode VisitDefaultSwitchLabel(DefaultSwitchLabelSyntax node)
        {
            bool shouldContinue = true;
            if (node is TNode && (node!=root || allowRootSelection))
                node = Mark(node, out shouldContinue);
            if (shouldContinue)
                return base.VisitDefaultSwitchLabel(node);
            else return node;
        }

        public override SyntaxNode VisitSwitchExpression(SwitchExpressionSyntax node)
        {
            bool shouldContinue = true;
            if (node is TNode && (node!=root || allowRootSelection))
                node = Mark(node, out shouldContinue);
            if (shouldContinue)
                return base.VisitSwitchExpression(node);
            else return node;
        }

        public override SyntaxNode VisitSwitchExpressionArm(SwitchExpressionArmSyntax node)
        {
            bool shouldContinue = true; 
            if (node is TNode && (node!=root || allowRootSelection)) 
                node = Mark(node, out shouldContinue);
            if (shouldContinue)
                return base.VisitSwitchExpressionArm(node);
            else return node;
        }

        public override SyntaxNode VisitTryStatement(TryStatementSyntax node)
        {
            bool shouldContinue = true;
            if (node is TNode && (node!=root || allowRootSelection))
                node = Mark(node, out shouldContinue); 
            if (shouldContinue)
                return base.VisitTryStatement(node);
            else return node;
        }

        public override SyntaxNode VisitCatchClause(CatchClauseSyntax node)
        {
            bool shouldContinue = true; 
            if (node is TNode && (node!=root || allowRootSelection)) 
                node = Mark(node, out shouldContinue);
            if (shouldContinue)
                return base.VisitCatchClause(node);
            else return node;
        }

        public override SyntaxNode VisitCatchDeclaration(CatchDeclarationSyntax node)
        {
            bool shouldContinue = true; 
            if (node is TNode && (node!=root || allowRootSelection)) 
                node = Mark(node, out shouldContinue);
            if (shouldContinue)
                return base.VisitCatchDeclaration(node);
            else return node;
        }

        public override SyntaxNode VisitCatchFilterClause(CatchFilterClauseSyntax node)
        {
            bool shouldContinue = true;
            if (node is TNode && (node!=root || allowRootSelection))
                node = Mark(node, out shouldContinue); 
            if (shouldContinue)
                return base.VisitCatchFilterClause(node);
            else return node;
        }

        public override SyntaxNode VisitFinallyClause(FinallyClauseSyntax node)
        {
            bool shouldContinue = true; 
            if (node is TNode && (node!=root || allowRootSelection)) 
                node = Mark(node, out shouldContinue); 
            if (shouldContinue)
                return base.VisitFinallyClause(node);
            else return node;
        }

        public override SyntaxNode VisitCompilationUnit(CompilationUnitSyntax node)
        {
            bool shouldContinue = true; 
            if (node is TNode && (node!=root || allowRootSelection)) 
                node = Mark(node, out shouldContinue);
            if (shouldContinue)
                return base.VisitCompilationUnit(node);
            else return node;
        }

        public override SyntaxNode VisitExternAliasDirective(ExternAliasDirectiveSyntax node)
        {
            bool shouldContinue = true;
            if (node is TNode && (node!=root || allowRootSelection))
                node = Mark(node, out shouldContinue);
            if (shouldContinue)
                return base.VisitExternAliasDirective(node);
            else return node;
        }

        public override SyntaxNode VisitUsingDirective(UsingDirectiveSyntax node)
        {
            bool shouldContinue = true; 
            if (node is TNode && (node!=root || allowRootSelection)) 
                node = Mark(node, out shouldContinue);
            if (shouldContinue)
                return base.VisitUsingDirective(node);
            else return node;
        }

        public override SyntaxNode VisitNamespaceDeclaration(NamespaceDeclarationSyntax node)
        {
            bool shouldContinue = true;
            if (node is TNode && (node!=root || allowRootSelection)) 
                node = Mark(node, out shouldContinue);
            if (shouldContinue)
                return base.VisitNamespaceDeclaration(node);
            else return node;
        }

        public override SyntaxNode VisitAttributeList(AttributeListSyntax node)
        {
            bool shouldContinue = true; 
            if (node is TNode && (node!=root || allowRootSelection))
                node = Mark(node, out shouldContinue); 
            if (shouldContinue)
                return base.VisitAttributeList(node);
            else return node;
        }

        public override SyntaxNode VisitAttributeTargetSpecifier(AttributeTargetSpecifierSyntax node)
        {
            bool shouldContinue = true;
            if (node is TNode && (node!=root || allowRootSelection))
                node = Mark(node, out shouldContinue);
            if (shouldContinue)
                return base.VisitAttributeTargetSpecifier(node);
            else return node;
        }

        public override SyntaxNode VisitAttribute(AttributeSyntax node)
        {
            bool shouldContinue = true;
            if (node is TNode && (node!=root || allowRootSelection)) 
                node = Mark(node, out shouldContinue);
            if (shouldContinue)
                return base.VisitAttribute(node);
            else return node;
        }

        public override SyntaxNode VisitAttributeArgumentList(AttributeArgumentListSyntax node)
        {
            bool shouldContinue = true; 
            if (node is TNode && (node!=root || allowRootSelection)) 
                node = Mark(node, out shouldContinue);
            if (shouldContinue)
                return base.VisitAttributeArgumentList(node);
            else return node;
        }

        public override SyntaxNode VisitAttributeArgument(AttributeArgumentSyntax node)
        {
            bool shouldContinue = true;
            if (node is TNode && (node!=root || allowRootSelection)) 
                node = Mark(node, out shouldContinue);
            if (shouldContinue)
                return base.VisitAttributeArgument(node);
            else return node;
        }

        public override SyntaxNode VisitNameEquals(NameEqualsSyntax node)
        {
            bool shouldContinue = true; 
            if (node is TNode && (node!=root || allowRootSelection)) 
                node = Mark(node, out shouldContinue); 
            if (shouldContinue)
                return base.VisitNameEquals(node);
            else return node;
        }

        public override SyntaxNode VisitTypeParameterList(TypeParameterListSyntax node)
        {
            bool shouldContinue = true; 
            if (node is TNode && (node!=root || allowRootSelection)) 
                node = Mark(node, out shouldContinue);
            if (shouldContinue)
                return base.VisitTypeParameterList(node);
            else return node;
        }

        public override SyntaxNode VisitTypeParameter(TypeParameterSyntax node)
        {
            bool shouldContinue = true;
            if (node is TNode && (node!=root || allowRootSelection)) 
                node = Mark(node, out shouldContinue);
            if (shouldContinue)
                return base.VisitTypeParameter(node);
            else return node;
        }

        public override SyntaxNode VisitClassDeclaration(ClassDeclarationSyntax node)
        {
            bool shouldContinue = true;
            if (node is TNode && (node!=root || allowRootSelection))
                node = Mark(node, out shouldContinue);
            if (shouldContinue)
                return base.VisitClassDeclaration(node);
            else return node;
        }

        public override SyntaxNode VisitStructDeclaration(StructDeclarationSyntax node)
        {
            bool shouldContinue = true;
            if (node is TNode && (node!=root || allowRootSelection)) 
                node = Mark(node, out shouldContinue);
            if (shouldContinue)
                return base.VisitStructDeclaration(node);
            else return node;
        }

        public override SyntaxNode VisitInterfaceDeclaration(InterfaceDeclarationSyntax node)
        {
            bool shouldContinue = true; 
            if (node is TNode && (node!=root || allowRootSelection)) 
                node = Mark(node, out shouldContinue);
            if (shouldContinue)
                return base.VisitInterfaceDeclaration(node);
            else return node;
        }

        public override SyntaxNode VisitEnumDeclaration(EnumDeclarationSyntax node)
        {
            bool shouldContinue = true;
            if (node is TNode && (node!=root || allowRootSelection))
                node = Mark(node, out shouldContinue); 
            if (shouldContinue)
                return base.VisitEnumDeclaration(node);
            else return node;
        }

        public override SyntaxNode VisitDelegateDeclaration(DelegateDeclarationSyntax node)
        {
            bool shouldContinue = true; 
            if (node is TNode && (node!=root || allowRootSelection)) 
                node = Mark(node, out shouldContinue); 
            if (shouldContinue)
                return base.VisitDelegateDeclaration(node);
            else return node;
        }

        public override SyntaxNode VisitEnumMemberDeclaration(EnumMemberDeclarationSyntax node)
        {
            bool shouldContinue = true; 
            if (node is TNode && (node!=root || allowRootSelection)) 
                node = Mark(node, out shouldContinue); 
            if (shouldContinue)
                return base.VisitEnumMemberDeclaration(node);
            else return node;
        }

        public override SyntaxNode VisitBaseList(BaseListSyntax node)
        {
            bool shouldContinue = true;
            if (node is TNode && (node!=root || allowRootSelection))
                node = Mark(node, out shouldContinue); 
            if (shouldContinue)
                return base.VisitBaseList(node);
            else return node;
        }

        public override SyntaxNode VisitSimpleBaseType(SimpleBaseTypeSyntax node)
        {
            bool shouldContinue = true; 
            if (node is TNode && (node!=root || allowRootSelection)) node =
                    Mark(node, out shouldContinue); 
            if (shouldContinue)
                return base.VisitSimpleBaseType(node);
            else return node;
        }

        public override SyntaxNode VisitTypeParameterConstraintClause(TypeParameterConstraintClauseSyntax node)
        {
            bool shouldContinue = true;
            if (node is TNode && (node!=root || allowRootSelection)) 
                node = Mark(node, out shouldContinue); 
            if (shouldContinue)
                return base.VisitTypeParameterConstraintClause(node);
            else return node;
        }

        public override SyntaxNode VisitConstructorConstraint(ConstructorConstraintSyntax node)
        {
            bool shouldContinue = true;
            if (node is TNode && (node!=root || allowRootSelection)) 
                node = Mark(node, out shouldContinue);
            if (shouldContinue)
                return base.VisitConstructorConstraint(node);
            else return node;
        }

        public override SyntaxNode VisitClassOrStructConstraint(ClassOrStructConstraintSyntax node)
        {
            bool shouldContinue = true;
            if (node is TNode && (node!=root || allowRootSelection)) 
                node = Mark(node, out shouldContinue);
            if (shouldContinue)
                return base.VisitClassOrStructConstraint(node);
            else return node;
        }

        public override SyntaxNode VisitTypeConstraint(TypeConstraintSyntax node)
        {
            bool shouldContinue = true; 
            if (node is TNode && (node!=root || allowRootSelection))
                node = Mark(node, out shouldContinue);
            if (shouldContinue)
                return base.VisitTypeConstraint(node);
            else return node;
        }

        public override SyntaxNode VisitFieldDeclaration(FieldDeclarationSyntax node)
        {
            bool shouldContinue = true; 
            if (node is TNode && (node!=root || allowRootSelection))
                node = Mark(node, out shouldContinue); 
            if (shouldContinue)
                return base.VisitFieldDeclaration(node);
            else return node;
        }

        public override SyntaxNode VisitEventFieldDeclaration(EventFieldDeclarationSyntax node)
        {
            bool shouldContinue = true; 
            if (node is TNode && (node!=root || allowRootSelection))
                node = Mark(node, out shouldContinue);
            if (shouldContinue)
                return base.VisitEventFieldDeclaration(node);
            else return node;
        }

        public override SyntaxNode VisitExplicitInterfaceSpecifier(ExplicitInterfaceSpecifierSyntax node)
        {
            bool shouldContinue = true; 
            if (node is TNode && (node!=root || allowRootSelection)) 
                node = Mark(node, out shouldContinue); 
            if (shouldContinue)
                return base.VisitExplicitInterfaceSpecifier(node);
            else return node;
        }

        public override SyntaxNode VisitMethodDeclaration(MethodDeclarationSyntax node)
        {
            bool shouldContinue = true; 
            if (node is TNode && (node!=root || allowRootSelection)) 
                node = Mark(node, out shouldContinue); 
            if (shouldContinue)
                return base.VisitMethodDeclaration(node);
            else return node;
        }

        public override SyntaxNode VisitOperatorDeclaration(OperatorDeclarationSyntax node)
        {
            bool shouldContinue = true; 
            if (node is TNode && (node!=root || allowRootSelection))
                node = Mark(node, out shouldContinue);
            if (shouldContinue)
                return base.VisitOperatorDeclaration(node);
            else return node;
        }

        public override SyntaxNode VisitConversionOperatorDeclaration(ConversionOperatorDeclarationSyntax node)
        {
            bool shouldContinue = true; 
            if (node is TNode && (node!=root || allowRootSelection)) 
                node = Mark(node, out shouldContinue);
            if (shouldContinue)
                return base.VisitConversionOperatorDeclaration(node);
            else return node;
        }

        public override SyntaxNode VisitConstructorDeclaration(ConstructorDeclarationSyntax node)
        {
            bool shouldContinue = true; 
            if (node is TNode && (node!=root || allowRootSelection)) 
                node = Mark(node, out shouldContinue);
            if (shouldContinue)
                return base.VisitConstructorDeclaration(node);
            else return node;
        }

        public override SyntaxNode VisitConstructorInitializer(ConstructorInitializerSyntax node)
        {
            bool shouldContinue = true;
            if (node is TNode && (node!=root || allowRootSelection)) 
                node = Mark(node, out shouldContinue);
            if (shouldContinue)
                return base.VisitConstructorInitializer(node);
            else return node;
        }

        public override SyntaxNode VisitDestructorDeclaration(DestructorDeclarationSyntax node)
        {
            bool shouldContinue = true; 
            if (node is TNode && (node!=root || allowRootSelection)) 
                node = Mark(node, out shouldContinue);
            if (shouldContinue)
                return base.VisitDestructorDeclaration(node);
            else return node;
        }

        public override SyntaxNode VisitPropertyDeclaration(PropertyDeclarationSyntax node)
        {
            bool shouldContinue = true; 
            if (node is TNode && (node!=root || allowRootSelection)) 
                node = Mark(node, out shouldContinue);
            if (shouldContinue)
                return base.VisitPropertyDeclaration(node);
            else return node;
        }

        public override SyntaxNode VisitArrowExpressionClause(ArrowExpressionClauseSyntax node)
        {
            bool shouldContinue = true;
            if (node is TNode && (node!=root || allowRootSelection)) 
                node = Mark(node, out shouldContinue);
            if (shouldContinue)
                return base.VisitArrowExpressionClause(node);
            else return node;
        }

        public override SyntaxNode VisitEventDeclaration(EventDeclarationSyntax node)
        {
            bool shouldContinue = true; 
            if (node is TNode && (node!=root || allowRootSelection)) 
                node = Mark(node, out shouldContinue);
            if (shouldContinue)
                return base.VisitEventDeclaration(node);
            else return node;
        }

        public override SyntaxNode VisitIndexerDeclaration(IndexerDeclarationSyntax node)
        {
            bool shouldContinue = true;
            if (node is TNode && (node!=root || allowRootSelection)) 
                node = Mark(node, out shouldContinue);
            if (shouldContinue)
                return base.VisitIndexerDeclaration(node);
            else return node;
        }

        public override SyntaxNode VisitAccessorList(AccessorListSyntax node)
        {
            bool shouldContinue = true; 
            if (node is TNode && (node!=root || allowRootSelection)) 
                node = Mark(node, out shouldContinue);
            if (shouldContinue)
                return base.VisitAccessorList(node);
            else return node;
        }

        public override SyntaxNode VisitAccessorDeclaration(AccessorDeclarationSyntax node)
        {
            bool shouldContinue = true; 
            if (node is TNode && (node!=root || allowRootSelection))
                node = Mark(node, out shouldContinue); 
            if (shouldContinue)
                return base.VisitAccessorDeclaration(node);
            else return node;
        }

        public override SyntaxNode VisitParameterList(ParameterListSyntax node)
        {
            bool shouldContinue = true; 
            if (node is TNode && (node!=root || allowRootSelection)) 
                node = Mark(node, out shouldContinue);
            if (shouldContinue)
                return base.VisitParameterList(node);
            else return node;
        }

        public override SyntaxNode VisitBracketedParameterList(BracketedParameterListSyntax node)
        {
            bool shouldContinue = true; 
            if (node is TNode && (node!=root || allowRootSelection)) 
                node = Mark(node, out shouldContinue);
            if (shouldContinue)
                return base.VisitBracketedParameterList(node);
            else return node;
        }

        public override SyntaxNode VisitParameter(ParameterSyntax node)
        {
            bool shouldContinue = true; 
            if (node is TNode && (node!=root || allowRootSelection)) 
                node = Mark(node, out shouldContinue);
            if (shouldContinue)
                return base.VisitParameter(node);
            else return node;
        }

        public override SyntaxNode VisitIncompleteMember(IncompleteMemberSyntax node)
        {
            bool shouldContinue = true;
            if (node is TNode && (node!=root || allowRootSelection)) 
                node = Mark(node, out shouldContinue);
            if (shouldContinue)
                return base.VisitIncompleteMember(node);
            else return node;
        }

        public override SyntaxNode VisitSkippedTokensTrivia(SkippedTokensTriviaSyntax node)
        {
            bool shouldContinue = true;
            if (node is TNode && (node!=root || allowRootSelection)) 
                node = Mark(node, out shouldContinue); 
            if (shouldContinue)
                return base.VisitSkippedTokensTrivia(node);
            else return node;
        }

        public override SyntaxNode VisitDocumentationCommentTrivia(DocumentationCommentTriviaSyntax node)
        {
            bool shouldContinue = true;
            if (node is TNode && (node!=root || allowRootSelection)) 
                node = Mark(node, out shouldContinue);
            if (shouldContinue)
                return base.VisitDocumentationCommentTrivia(node);
            else return node;
        }

        public override SyntaxNode VisitTypeCref(TypeCrefSyntax node)
        {
            bool shouldContinue = true; 
            if (node is TNode && (node!=root || allowRootSelection))
                node = Mark(node, out shouldContinue); 
            if (shouldContinue)
                return base.VisitTypeCref(node);
            else return node;
        }

        public override SyntaxNode VisitQualifiedCref(QualifiedCrefSyntax node)
        {
            bool shouldContinue = true; 
            if (node is TNode && (node!=root || allowRootSelection)) 
                node = Mark(node, out shouldContinue);
            if (shouldContinue)
                return base.VisitQualifiedCref(node);
            else return node;
        }

        public override SyntaxNode VisitNameMemberCref(NameMemberCrefSyntax node)
        {
            bool shouldContinue = true; 
            if (node is TNode && (node!=root || allowRootSelection)) 
                node = Mark(node, out shouldContinue);
            if (shouldContinue)
                return base.VisitNameMemberCref(node);
            else return node;
        }

        public override SyntaxNode VisitIndexerMemberCref(IndexerMemberCrefSyntax node)
        {
            bool shouldContinue = true;
            if (node is TNode && (node!=root || allowRootSelection))
                node = Mark(node, out shouldContinue);
            if (shouldContinue)
                return base.VisitIndexerMemberCref(node);
            else return node;
        }

        public override SyntaxNode VisitOperatorMemberCref(OperatorMemberCrefSyntax node)
        {
            bool shouldContinue = true; 
            if (node is TNode && (node!=root || allowRootSelection)) 
                node = Mark(node, out shouldContinue); 
            if (shouldContinue)
                return base.VisitOperatorMemberCref(node);
            else return node;
        }

        public override SyntaxNode VisitConversionOperatorMemberCref(ConversionOperatorMemberCrefSyntax node)
        {
            bool shouldContinue = true; 
            if (node is TNode && (node!=root || allowRootSelection))
                node = Mark(node, out shouldContinue);
            if (shouldContinue)
                return base.VisitConversionOperatorMemberCref(node);
            else return node;
        }

        public override SyntaxNode VisitCrefParameterList(CrefParameterListSyntax node)
        {
            bool shouldContinue = true;
            if (node is TNode && (node!=root || allowRootSelection))
                node = Mark(node, out shouldContinue);
            if (shouldContinue)
                return base.VisitCrefParameterList(node);
            else return node;
        }

        public override SyntaxNode VisitCrefBracketedParameterList(CrefBracketedParameterListSyntax node)
        {
            bool shouldContinue = true; 
            if (node is TNode && (node!=root || allowRootSelection)) 
                node = Mark(node, out shouldContinue);
            if (shouldContinue)
                return base.VisitCrefBracketedParameterList(node);
            else return node;
        }

        public override SyntaxNode VisitCrefParameter(CrefParameterSyntax node)
        {
            bool shouldContinue = true; 
            if (node is TNode && (node!=root || allowRootSelection)) 
                node = Mark(node, out shouldContinue); 
            if (shouldContinue)
                return base.VisitCrefParameter(node);
            else return node;
        }

        public override SyntaxNode VisitXmlElement(XmlElementSyntax node)
        {
            bool shouldContinue = true;
            if (node is TNode && (node!=root || allowRootSelection))
                node = Mark(node, out shouldContinue); 
            if (shouldContinue)
                return base.VisitXmlElement(node);
            else return node;
        }

        public override SyntaxNode VisitXmlElementStartTag(XmlElementStartTagSyntax node)
        {
            bool shouldContinue = true;
            if (node is TNode && (node!=root || allowRootSelection)) 
                node = Mark(node, out shouldContinue);
            if (shouldContinue)
                return base.VisitXmlElementStartTag(node);
            else return node;
        }

        public override SyntaxNode VisitXmlElementEndTag(XmlElementEndTagSyntax node)
        {
            bool shouldContinue = true;
            if (node is TNode && (node!=root || allowRootSelection)) 
                node = Mark(node, out shouldContinue);
            if (shouldContinue)
                return base.VisitXmlElementEndTag(node);
            else return node;
        }

        public override SyntaxNode VisitXmlEmptyElement(XmlEmptyElementSyntax node)
        {
            bool shouldContinue = true; 
            if (node is TNode && (node!=root || allowRootSelection)) 
                node = Mark(node, out shouldContinue);
            if (shouldContinue)
                return base.VisitXmlEmptyElement(node);
            else return node;
        }

        public override SyntaxNode VisitXmlName(XmlNameSyntax node)
        {
            bool shouldContinue = true; 
            if (node is TNode && (node!=root || allowRootSelection)) 
                node = Mark(node, out shouldContinue); 
            if (shouldContinue)
                return base.VisitXmlName(node);
            else return node;
        }

        public override SyntaxNode VisitXmlPrefix(XmlPrefixSyntax node)
        {
            bool shouldContinue = true; 
            if (node is TNode && (node!=root || allowRootSelection))
                node = Mark(node, out shouldContinue); 
            if (shouldContinue)
                return base.VisitXmlPrefix(node);
            else return node;
        }

        public override SyntaxNode VisitXmlTextAttribute(XmlTextAttributeSyntax node)
        {
            bool shouldContinue = true;
            if (node is TNode && (node!=root || allowRootSelection)) 
                node = Mark(node, out shouldContinue);
            if (shouldContinue)
                return base.VisitXmlTextAttribute(node);
            else return node;
        }

        public override SyntaxNode VisitXmlCrefAttribute(XmlCrefAttributeSyntax node)
        {
            bool shouldContinue = true; 
            if (node is TNode && (node!=root || allowRootSelection))
                node = Mark(node, out shouldContinue); 
            if (shouldContinue)
                return base.VisitXmlCrefAttribute(node);
            else return node;
        }

        public override SyntaxNode VisitXmlNameAttribute(XmlNameAttributeSyntax node)
        {
            bool shouldContinue = true; 
            if (node is TNode && (node!=root || allowRootSelection)) 
                node = Mark(node, out shouldContinue);
            if (shouldContinue)
                return base.VisitXmlNameAttribute(node);
            else return node;
        }

        public override SyntaxNode VisitXmlText(XmlTextSyntax node)
        {
            bool shouldContinue = true;
            if (node is TNode && (node!=root || allowRootSelection)) 
                node = Mark(node, out shouldContinue);
            if (shouldContinue)
                return base.VisitXmlText(node);
            else return node;
        }

        public override SyntaxNode VisitXmlCDataSection(XmlCDataSectionSyntax node)
        {
            bool shouldContinue = true; 
            if (node is TNode && (node!=root || allowRootSelection)) 
                node = Mark(node, out shouldContinue);
            if (shouldContinue)
                return base.VisitXmlCDataSection(node);
            else return node;
        }

        public override SyntaxNode VisitXmlProcessingInstruction(XmlProcessingInstructionSyntax node)
        {
            bool shouldContinue = true; 
            if (node is TNode && (node!=root || allowRootSelection))
                node = Mark(node, out shouldContinue); 
            if (shouldContinue)
                return base.VisitXmlProcessingInstruction(node);
            else return node;
        }

        public override SyntaxNode VisitXmlComment(XmlCommentSyntax node)
        {
            bool shouldContinue = true;
            if (node is TNode && (node!=root || allowRootSelection))
                node = Mark(node, out shouldContinue);
            if (shouldContinue)
                return base.VisitXmlComment(node);
            else return node;
        }

        public override SyntaxNode VisitIfDirectiveTrivia(IfDirectiveTriviaSyntax node)
        {
            bool shouldContinue = true;
            if (node is TNode && (node!=root || allowRootSelection))
                node = Mark(node, out shouldContinue); 
            if (shouldContinue)
                return base.VisitIfDirectiveTrivia(node);
            else return node;
        }

        public override SyntaxNode VisitElifDirectiveTrivia(ElifDirectiveTriviaSyntax node)
        {
            bool shouldContinue = true;
            if (node is TNode && (node!=root || allowRootSelection))
                node = Mark(node, out shouldContinue);
            if (shouldContinue)
                return base.VisitElifDirectiveTrivia(node);
            else return node;
        }

        public override SyntaxNode VisitElseDirectiveTrivia(ElseDirectiveTriviaSyntax node)
        {
            bool shouldContinue = true; 
            if (node is TNode && (node!=root || allowRootSelection)) 
                node = Mark(node, out shouldContinue);
            if (shouldContinue)
                return base.VisitElseDirectiveTrivia(node);
            else return node;
        }

        public override SyntaxNode VisitEndIfDirectiveTrivia(EndIfDirectiveTriviaSyntax node)
        {
            bool shouldContinue = true;
            if (node is TNode && (node!=root || allowRootSelection))
                node = Mark(node, out shouldContinue); 
            if (shouldContinue)
                return base.VisitEndIfDirectiveTrivia(node);
            else return node;
        }

        public override SyntaxNode VisitRegionDirectiveTrivia(RegionDirectiveTriviaSyntax node)
        {
            bool shouldContinue = true;
            if (node is TNode && (node!=root || allowRootSelection))
                node = Mark(node, out shouldContinue);
            if (shouldContinue)
                return base.VisitRegionDirectiveTrivia(node);
            else return node;
        }

        public override SyntaxNode VisitEndRegionDirectiveTrivia(EndRegionDirectiveTriviaSyntax node)
        {
            bool shouldContinue = true; 
            if (node is TNode && (node!=root || allowRootSelection)) 
                node = Mark(node, out shouldContinue);
            if (shouldContinue)
                return base.VisitEndRegionDirectiveTrivia(node);
            else return node;
        }

        public override SyntaxNode VisitErrorDirectiveTrivia(ErrorDirectiveTriviaSyntax node)
        {
            bool shouldContinue = true; 
            if (node is TNode && (node!=root || allowRootSelection)) 
                node = Mark(node, out shouldContinue);
            if (shouldContinue)
                return base.VisitErrorDirectiveTrivia(node);
            else return node;
        }

        public override SyntaxNode VisitWarningDirectiveTrivia(WarningDirectiveTriviaSyntax node)
        {
            bool shouldContinue = true; 
            if (node is TNode && (node!=root || allowRootSelection))
                node = Mark(node, out shouldContinue);
            if (shouldContinue)
                return base.VisitWarningDirectiveTrivia(node);
            else return node;
        }

        public override SyntaxNode VisitBadDirectiveTrivia(BadDirectiveTriviaSyntax node)
        {
            bool shouldContinue = true; 
            if (node is TNode && (node!=root || allowRootSelection))
                node = Mark(node, out shouldContinue);
            if (shouldContinue)
                return base.VisitBadDirectiveTrivia(node);
            else return node;
        }

        public override SyntaxNode VisitDefineDirectiveTrivia(DefineDirectiveTriviaSyntax node)
        {
            bool shouldContinue = true; 
            if (node is TNode && (node!=root || allowRootSelection)) 
                node = Mark(node, out shouldContinue);
            if (shouldContinue)
                return base.VisitDefineDirectiveTrivia(node);
            else return node;
        }

        public override SyntaxNode VisitUndefDirectiveTrivia(UndefDirectiveTriviaSyntax node)
        {
            bool shouldContinue = true;
            if (node is TNode && (node!=root || allowRootSelection))
                node = Mark(node, out shouldContinue); 
            if (shouldContinue)
                return base.VisitUndefDirectiveTrivia(node);
            else return node;
        }

        public override SyntaxNode VisitLineDirectiveTrivia(LineDirectiveTriviaSyntax node)
        {
            bool shouldContinue = true;
            if (node is TNode && (node!=root || allowRootSelection)) 
                node = Mark(node, out shouldContinue); 
            if (shouldContinue)
                return base.VisitLineDirectiveTrivia(node);
            else return node;
        }

        public override SyntaxNode VisitPragmaWarningDirectiveTrivia(PragmaWarningDirectiveTriviaSyntax node)
        {
            bool shouldContinue = true;
            if (node is TNode && (node!=root || allowRootSelection)) 
                node = Mark(node, out shouldContinue); 
            if (shouldContinue)
                return base.VisitPragmaWarningDirectiveTrivia(node);
            else return node;
        }

        public override SyntaxNode VisitPragmaChecksumDirectiveTrivia(PragmaChecksumDirectiveTriviaSyntax node)
        {
            bool shouldContinue = true; 
            if (node is TNode && (node!=root || allowRootSelection))
                node = Mark(node, out shouldContinue);
            if (shouldContinue)
                return base.VisitPragmaChecksumDirectiveTrivia(node);
            else return node;
        }

        public override SyntaxNode VisitReferenceDirectiveTrivia(ReferenceDirectiveTriviaSyntax node)
        {
            bool shouldContinue = true;
            if (node is TNode && (node!=root || allowRootSelection)) 
                node = Mark(node, out shouldContinue);
            if (shouldContinue)
                return base.VisitReferenceDirectiveTrivia(node);
            else return node;
        }

        public override SyntaxNode VisitLoadDirectiveTrivia(LoadDirectiveTriviaSyntax node)
        {
            bool shouldContinue = true; 
            if (node is TNode && (node!=root || allowRootSelection)) 
                node = Mark(node, out shouldContinue);
            if (shouldContinue)
                return base.VisitLoadDirectiveTrivia(node);
            else return node;
        }

        public override SyntaxNode VisitShebangDirectiveTrivia(ShebangDirectiveTriviaSyntax node)
        {
            bool shouldContinue = true; 
            if (node is TNode && (node!=root || allowRootSelection)) 
                node = Mark(node, out shouldContinue); 
            if (shouldContinue)
                return base.VisitShebangDirectiveTrivia(node);
            else return node;
        }

        public override SyntaxNode VisitNullableDirectiveTrivia(NullableDirectiveTriviaSyntax node)
        {
            bool shouldContinue = true; 
            if (node is TNode && (node!=root || allowRootSelection))
                node = Mark(node, out shouldContinue);
            if (shouldContinue)
                return base.VisitNullableDirectiveTrivia(node);
            else
                return node;
        }

    }
}
