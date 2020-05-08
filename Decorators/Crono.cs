using System;
using System.Diagnostics;

namespace Decorators
{
    public class Crono : DecoratorAttribute
    {
        protected override object Decorator(Delegate d, params object[] objects)
        {
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            var result = d.DynamicInvoke(objects);
            stopwatch.Stop();
            Console.WriteLine($"Method {d.Method.Name} run in {stopwatch.ElapsedMilliseconds}");
            return result;
        }
    }
}
