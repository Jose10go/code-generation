using System;
using System.Collections.Generic;

namespace CodeGen.Context
{
    // Abstract Typeless Context
    public partial class CodeGenTypelessContext
    {
        public interface ITarget
        {
            Type TargetNode { get; }

            Func<object, bool> WhereSelector { get; }

            IEnumerable<object> Select(object root);
        }
    }
}
