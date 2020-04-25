using System;
using System.Collections.Generic;
namespace Tests.Examples.CloneStruct
{
    public struct C
    {
        public void hello()
        {
            Console.WriteLine("hello my friend.");
        }
    }

    protected struct C_generated<TGeneric>:IEnumerable<string>
        where TGeneric:Console
    {
        public void hello()
        {
            Console.WriteLine("hello my friend.");
        }
    }
}
