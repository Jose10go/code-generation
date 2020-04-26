namespace CodeGen.Core
{
    public interface IKey
    {
        int Id { get; }
    }

    public sealed class Key<T> : IKey
        where T : class
    {
        public int Id { get; }
        internal Key(int id)
        {
            Id = id;
        }

        public override int GetHashCode()
        {
            return Id;
        }

        public override bool Equals(object obj)
        {
            if (obj is null || !(obj is IKey))
                return false;
            var other = obj as IKey;
            return Id == other.Id;
        }

        public override string ToString()
        {
            return $"Key<{typeof(T)}>-{Id}";
        }
    }
}
