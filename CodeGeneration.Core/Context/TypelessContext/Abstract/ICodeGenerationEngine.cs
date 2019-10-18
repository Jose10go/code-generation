namespace CodeGen.Context
{
    public partial class CodeGenTypelessContext
    {
        public interface ICodeGenerationEngine
        {
            object ApplyChanges();

            object CurrentSolution { get; }

            //IChainTargetBuilder Select<TSyntaxNode>();TODO:see this and in general review all typelessContext
        }
    }
}
