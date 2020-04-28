using System;

namespace FancyDecoTransformer
{
    [AttributeUsage(AttributeTargets.Method,AllowMultiple =true,Inherited=true)]
    public abstract class DecoratorAttribute:Attribute
    {
        public abstract Delegate Decorate(Delegate f);
    }
}
