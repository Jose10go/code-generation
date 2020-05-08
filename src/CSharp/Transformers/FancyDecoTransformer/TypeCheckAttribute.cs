using System;

namespace FancyDecoTransformer
{
    public class TypeCheckAttribute:Attribute
    {
        public Type Type { get; }
        public int Position { get; }
        public TypeCheckAttribute(Type type,int position)
        {
            this.Type = type;
            this.Position = position;
        }
    }
}
