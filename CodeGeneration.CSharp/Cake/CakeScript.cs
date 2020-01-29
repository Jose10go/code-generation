using System;
using Cake.Core;
using Cake.Core.Annotations;

namespace CodeGeneration.CSharp.Cake
{
    public static class  CakeScript
    {
        [CakeMethodAlias]
        public static void Precompile(this ICakeContext context)
        {

        }

        //[CakePropertyAlias]
        //public static int TheAnswerToLife(this ICakeContext context)
        //{
        //    return 42;
        //}
    }
}
