using CodeGen.DI.Abstract;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.Editing;
using System;

namespace CodeGen.Context.CSharp
{
    public partial class ICSharpContext<TProcessEntity> : CodeGen.Context.CodeGenContext<Solution, CSharpSyntaxNode, TProcessEntity>
    {
        public class DocumentEditingCodeGenerationEngine : ICSharpCodeGenerationEngine
        {
            public Solution CurrentSolution { get; private set; }

            protected SolutionEditor Editor { get; private set; }

            protected ICodeGenerationResolver<Solution, CSharpSyntaxNode, DocumentEditor> Resolver { get; private set; }

            object CodeGenTypelessContext.ICodeGenerationEngine.CurrentSolution => CurrentSolution;

            public DocumentEditingCodeGenerationEngine(Solution solution, ICodeGenerationResolver<Solution, CSharpSyntaxNode, DocumentEditor> resolver)
            {
                Editor = new SolutionEditor(solution);
                Resolver = resolver;
                resolver.BuildContainer();
            }

            public Solution ApplyChanges()
            {
                return Editor.GetChangedSolution();
            }

            object CodeGenTypelessContext.ICodeGenerationEngine.ApplyChanges()
            {
                return Editor.GetChangedSolution();
            }

            ICSharpTargetBuilder<TSyntaxNode> ICSharpCodeGenerationEngine.Select<TSyntaxNode>()
            {
                throw new NotImplementedException();
            }

            ITargetBuilder<TSyntaxNode> ICodeGenerationEngine.Select<TSyntaxNode>()
            {
                throw new NotImplementedException();
            }

            public ITargetBuilder Select<TSyntaxNode>()
            {
                throw new NotImplementedException();
            }
        }
    }
}