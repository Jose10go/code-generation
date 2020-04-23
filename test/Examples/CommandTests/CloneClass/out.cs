using System;
using System.Collections.Generic;
namespace Tests.Examples.CloneClass
{
    public class A
    {
        public void hello()
        {
            Console.WriteLine("hello my friend.");
        }
    }

    public class A_generated<TGeneric>:IEnumerable<string>
        where TGeneric:Console
    {
        public void hello()
        {
            Console.WriteLine("hello my friend.");
        }
    }
}
