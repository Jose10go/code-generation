using System;
using System.Collections.Generic;
namespace Tests.Examples.ModifyClass
{
    public class A_modified<TGeneric>:ICollection<string>
        where TGeneric:struct
    {
        public void hello()
        {
            Console.WriteLine("hello my friend.");
        }
    }
}
