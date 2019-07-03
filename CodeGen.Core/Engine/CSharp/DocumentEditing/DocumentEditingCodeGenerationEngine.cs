using CodeGen.Commands;
using CodeGen.Commands.Abstract;
using CodeGen.DI.Abstract;
using CodeGen.Engine.Abstract;
using CodeGen.Engine.CSharp.DocumentEditing;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.Editing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeGen.Engine.CSharp
{
    public class DocumentEditingCodeGenerationEngine : ICSharpCodeGenerationEngine<DocumentEditor>
    {
        public Solution CurrentSolution { get; private set; }
        object ICodeGenerationEngine.CurrentSolution => CurrentSolution;

        protected SolutionEditor Editor { get; private set; }

        protected ICodeGenerationResolver<Solution, CSharpSyntaxNode, DocumentEditor> Resolver { get; private set; }

        public DocumentEditingCodeGenerationEngine(Solution solution, ICodeGenerationResolver<Solution, CSharpSyntaxNode, DocumentEditor> resolver)
        {
            Editor = new SolutionEditor(solution);
            Resolver = resolver;
            resolver.BuildContainer();
        }      

        public Solution ApplyChanges()
        {
            throw new NotImplementedException();
        }

        object ICodeGenerationEngine.ApplyChanges()
        {
            return ApplyChanges();
        }

        public ICSharpTargetBuilder<TSyntaxNode, DocumentEditor> Select<TSyntaxNode>() where TSyntaxNode : CSharpSyntaxNode
        {
            return (ICSharpTargetBuilder<TSyntaxNode, DocumentEditor>)Resolver.ResolveTargetBuilder<TSyntaxNode>();
        }

        ITargetBuilder ICodeGenerationEngine.Select<TSyntaxNode>()
        {
            return Resolver.ResolveTargetBuilder<TSyntaxNode>();
        }

        ITargetBuilder<TSyntaxNode, CSharpSyntaxNode, DocumentEditor> ICodeGenerationEngine<Solution, CSharpSyntaxNode, DocumentEditor>.Select<TSyntaxNode>()
        {
            return Select<TSyntaxNode>();
        }
    }
}
