using CodeGen.Attributes;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Editing;
using CodeGen.Context;
using Microsoft.CodeAnalysis.CSharp;

namespace CodeGen.CSharp.Context
{
    public abstract partial class CSharpContext : CodeGenContext<Project, CSharpSyntaxNode, CompilationUnitSyntax, ISymbol>
    {
        [CommandHandler]
        public class ReplaceExpressionCommandHandler<Exp> : CommandHandler<IReplaceExpression<Exp>>
            where Exp:ExpressionSyntax
        {
            public ReplaceExpressionCommandHandler(IReplaceExpression<Exp> command) : base(command)
            {
            }

            private void VisitExpression(ExpressionSyntax node)
            {
                var newNode= this.Command.NewExpression
                                         .WithAdditionalAnnotations(new SyntaxAnnotation($"{Id}"));
                DocumentEditor.ReplaceNode(node,newNode);
            }

            public override void VisitArrayCreationExpression(ArrayCreationExpressionSyntax node)
            {
                VisitExpression(node);
            }

            public override void VisitAnonymousMethodExpression(AnonymousMethodExpressionSyntax node)
            {
                VisitExpression(node);
            }

            public override void VisitAnonymousObjectCreationExpression(AnonymousObjectCreationExpressionSyntax node)
            {
                VisitExpression(node);
            }

            public override void VisitAssignmentExpression(AssignmentExpressionSyntax node)
            {
                VisitExpression(node);
            }

            public override void VisitAwaitExpression(AwaitExpressionSyntax node)
            {
                VisitExpression(node);
            }

            public override void VisitBaseExpression(BaseExpressionSyntax node)
            {
                VisitExpression(node);
            }

            public override void VisitBinaryExpression(BinaryExpressionSyntax node)
            {
                VisitExpression(node);
            }

            public override void VisitCastExpression(CastExpressionSyntax node)
            {
                VisitExpression(node);
            }

            public override void VisitCheckedExpression(CheckedExpressionSyntax node)
            {
                this.VisitExpression(node);
            }

            public override void VisitConditionalAccessExpression(ConditionalAccessExpressionSyntax node)
            {
                this.VisitExpression(node);
            }

            public override void VisitConditionalExpression(ConditionalExpressionSyntax node)
            {
                this.VisitExpression(node);
            }

            public override void VisitDefaultExpression(DefaultExpressionSyntax node)
            {
                this.VisitExpression(node);
            }

            public override void VisitDeclarationExpression(DeclarationExpressionSyntax node)
            {
                this.VisitExpression(node);
            }

            public override void VisitElementAccessExpression(ElementAccessExpressionSyntax node)
            {
                this.VisitExpression(node);
            }

            public override void VisitElementBindingExpression(ElementBindingExpressionSyntax node)
            {
                this.VisitExpression(node);
            }

            public override void VisitImplicitArrayCreationExpression(ImplicitArrayCreationExpressionSyntax node)
            {
                this.VisitExpression(node);
            }

            public override void VisitInitializerExpression(InitializerExpressionSyntax node)
            {
                this.VisitExpression(node);
            }

            public override void VisitInterpolatedStringExpression(InterpolatedStringExpressionSyntax node)
            {
                this.VisitExpression(node);
            }

            public override void VisitInvocationExpression(InvocationExpressionSyntax node)
            {
                this.VisitExpression(node);
            }

            public override void VisitIsPatternExpression(IsPatternExpressionSyntax node)
            {
                this.VisitExpression(node);
            }

            public override void VisitLiteralExpression(LiteralExpressionSyntax node)
            {
                this.VisitExpression(node);
            }

            public override void VisitMemberAccessExpression(MemberAccessExpressionSyntax node)
            {
                this.VisitExpression(node);
            }

            public override void VisitParenthesizedExpression(ParenthesizedExpressionSyntax node)
            {
                this.VisitExpression(node);
            }

            public override void VisitTupleExpression(TupleExpressionSyntax node)
            {
                this.VisitExpression(node);
            }

            public override void VisitPrefixUnaryExpression(PrefixUnaryExpressionSyntax node)
            {
                this.VisitExpression(node);
            }

            public override void VisitPostfixUnaryExpression(PostfixUnaryExpressionSyntax node)
            {
                this.VisitExpression(node);
            }

            public override void VisitMemberBindingExpression(MemberBindingExpressionSyntax node)
            {
                this.VisitExpression(node);
            }

            public override void VisitRangeExpression(RangeExpressionSyntax node)
            {
                this.VisitExpression(node);
            }

            public override void VisitThisExpression(ThisExpressionSyntax node)
            {
                this.VisitExpression(node);
            }

            public override void VisitTypeOfExpression(TypeOfExpressionSyntax node)
            {
                this.VisitExpression(node);
            }

            public override void VisitSizeOfExpression(SizeOfExpressionSyntax node)
            {
                this.VisitExpression(node);
            }

            public override void VisitSimpleLambdaExpression(SimpleLambdaExpressionSyntax node)
            {
                this.VisitExpression(node);
            }

            public override void VisitRefExpression(RefExpressionSyntax node)
            {
                this.VisitExpression(node);
            }

            public override void VisitParenthesizedLambdaExpression(ParenthesizedLambdaExpressionSyntax node)
            {
                this.VisitExpression(node);
            }

            public override void VisitObjectCreationExpression(ObjectCreationExpressionSyntax node)
            {
                this.VisitExpression(node);
            }

            public override void VisitQueryExpression(QueryExpressionSyntax node)
            {
                this.VisitExpression(node);
            }

            public override void VisitThrowExpression(ThrowExpressionSyntax node)
            {
                this.VisitExpression(node);
            }

            public override void VisitSwitchExpression(SwitchExpressionSyntax node)
            {
                this.VisitExpression(node);
            }

        }
    }
}