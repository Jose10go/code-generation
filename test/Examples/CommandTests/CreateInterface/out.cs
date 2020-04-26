using System;
using System.Collections.Generic;

namespace Tests.Examples.CreateInterface
{
    internal partial interface IA<T>:IDictionary<string,T>,ICollection<T>
        where T:IEnumerable<string>
    {
        IA<T> Parse();
        string ToString();
    }
}