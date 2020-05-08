using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using FancyDecoTransformer;
namespace Decorators
{
    public class Memoize : Decorator
    {
        private class Comparer : IEqualityComparer<object[]>
        {
            public bool Equals([AllowNull] object[] x, [AllowNull] object[] y)
            {
                if (x is null && y is null)
                    return true;
                if (x is null || y is null)
                    return false;
                if (x.Length != y.Length)
                    return false;
                for (int i = 0; i < x.Length; i++)
                    if (!x[i]?.Equals(y[i]) ?? y[i] == null)
                        return false;
                return true;
            }

            public int GetHashCode([DisallowNull] object[] obj)
            {
                return obj?.Sum(x => x?.GetHashCode() ?? 0) ?? 0;
            }
        }

        readonly Dictionary<object[], object> dictionary = new Dictionary<object[], object>(new Comparer());

        protected override object Decorate(Delegate d, params object[] objects)
        {
            if (dictionary.ContainsKey(objects))
                return dictionary[objects];
            return d.DynamicInvoke(objects);
        }

    }
}
