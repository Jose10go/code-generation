using System;
using System.Linq;

namespace Decorators
{
    public class Logg : Decorator
    {
        protected override object Decorator(Delegate d, params object[] objects)
        {
            Console.WriteLine($"Starting execution of {d.Method.Name} with arguments {String.Join(',', objects.Select(x => x?.ToString() ?? "<null>"))}.");
            var res = d.DynamicInvoke(objects);
            Console.WriteLine($"Ending execution of {d.Method.Name} with arguments {String.Join(',', objects.Select(x => x?.ToString() ?? "<null>"))}.");
            return res;
        }
    }
}
