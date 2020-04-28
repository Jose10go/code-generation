
namespace CodeGen.Core
{
    public interface ISingleTarget:ITarget
    {
        T Get<T>(Key<T> key);
    }
}
