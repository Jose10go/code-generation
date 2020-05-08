using System;
using System.Diagnostics;
using FancyDecoTransformer;
namespace Decorators
{
    public class Crono : Decorator
    {
        protected override object Decorate(Delegate d, params object[] objects)
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
