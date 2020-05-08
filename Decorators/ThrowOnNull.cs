using FancyDecoTransformer;
using System;

namespace Decorators
{
    public class ThrowOnNull : Decorator
    {
        protected override object Decorate(Delegate d, params object[] objects)
        {
            var parameters = d.Method.GetParameters();
            for (int i = 0; i < objects.Length; i++)
                if (objects[i] is null)
                    throw new ArgumentNullException($"Check that parameter {parameters[i].Name} is not null.");
            return d.DynamicInvoke(objects);
        }
    }
}
