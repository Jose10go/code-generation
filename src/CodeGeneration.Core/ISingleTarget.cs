
namespace CodeGen.Core
{
    public interface ISingleTarget:ITarget
    {
        void Get<T>(Key<T> key, out T value);
    }
}
