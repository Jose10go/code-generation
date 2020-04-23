using System;
using System.Collections.Generic;

namespace Tests.Examples.CreateClass
{
    public static partial class A<T>:Dictionary<string,T>,ICollection<T>,IComparable<T>
        where T:IEnumerable<T>
    {
    }
}