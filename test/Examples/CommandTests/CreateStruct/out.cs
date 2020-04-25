using System;
using System.Collections.Generic;

namespace Tests.Examples.CreateStruct
{
    public partial struct A<T>:IDictionary<string,T>,ICollection<T>,IComparable<T>
        where T:IEnumerable<T>
    {
    }
}