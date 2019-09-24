namespace CodeGen.Context
{
    public partial class CodeGenTypelessContext
    {
        public interface ICodeGenerationEngine
        {
            object ApplyChanges();

            object CurrentSolution { get; }

            ITargetBuilder Select<TSyntaxNode>();
        }
    }
}
