using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.Editing;

namespace CodeGen.Context.CSharp
{
    public partial class ICSharpContext<TProcessEntity> : CodeGenContext<Solution, CSharpSyntaxNode, TProcessEntity>
    {
        public class DocumentEditingCodeGenerationEngine : ICSharpCodeGenerationEngine
        {
            public Solution CurrentSolution { get; private set; }

            object CodeGenTypelessContext.ICodeGenerationEngine.CurrentSolution => CurrentSolution;

            protected SolutionEditor Editor { get; private set; }

            protected AutofacResolver Resolver { get; private set; }

            public DocumentEditingCodeGenerationEngine(Solution solution, AutofacResolver resolver)
            {
                Editor = new SolutionEditor(solution);
                Resolver = resolver;
                resolver.RegisterEngine(this);
                resolver.BuildContainer();
            }

            public Solution ApplyChanges()
            {
                return Editor.GetChangedSolution();
            }

            object CodeGenTypelessContext.ICodeGenerationEngine.ApplyChanges()
            {
                return ApplyChanges();
            }

            public ICSharpTargetBuilder<TSyntaxNode> Select<TSyntaxNode>()where TSyntaxNode :CSharpSyntaxNode
            {
                return Resolver.ResolveTargetBuilder<TSyntaxNode>() as ICSharpTargetBuilder<TSyntaxNode>;
            }
            ITargetBuilder<TSyntaxNode> ICodeGenerationEngine.Select<TSyntaxNode>()
            {
                return Resolver.ResolveTargetBuilder<TSyntaxNode>();
            }
            ITargetBuilder CodeGenTypelessContext.ICodeGenerationEngine.Select<TSyntaxNode>()
            {
                return Resolver.ResolveTargetBuilder<TSyntaxNode>();
            }

        }
    }
}