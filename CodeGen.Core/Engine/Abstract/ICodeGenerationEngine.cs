using CodeGen.Commands.Abstract;

namespace CodeGen.Engine.Abstract
{
    public interface ICodeGenerationEngine
    {
        object ApplyChanges();

        object CurrentSolution { get; }

        ITargetBuilder Select<TSyntaxNode>();
    }

    public interface ICodeGenerationEngine<TSolution, TRoot, TProcessEntity> : ICodeGenerationEngine
    {
        new TSolution ApplyChanges();

        new TSolution CurrentSolution { get; }

        new ITargetBuilder<TSyntaxNode, TRoot, TProcessEntity> Select<TSyntaxNode>() where TSyntaxNode : TRoot;
    }
}
