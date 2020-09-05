using FancyDecoCore;
using System;

namespace Decorators
{
    public class TagsComposition : Decorator
    {
        readonly string tag;
        public TagsComposition(string tag)
        {
            this.tag = tag;
        }

        protected override object Decorate(Delegate d, params object[] objects)
        {
            return "<" + tag + ">\n" + d.DynamicInvoke(objects)?.ToString() ?? "" + " \n<" + tag + "/>";
        }

    }
}
