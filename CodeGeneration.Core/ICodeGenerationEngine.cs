using System;

namespace CodeGen.Core
{
    public interface ICodeGenerationEngine
    {
        void ApplyChanges<TCommandHandler>(TCommandHandler commandHandler)
            where TCommandHandler : ICommandHandler;
    }
}
