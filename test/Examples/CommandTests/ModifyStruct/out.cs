using System;
using System.Collections.Generic;
namespace Tests.Examples.ModifyStruct
{
    protected struct C_modified<TGeneric>:ICollection<double>
        where TGeneric:struct
    {
        public void hello()
        {
            Console.WriteLine("hello my friend.");
        }
    }
}
