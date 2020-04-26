namespace Tests.Examples.CloneInterface
{
    public interface I
    {
        void hello();
    }

    internal interface I_generated<T>
        where T : class
    {
        void goodBye();
        void hello();
    }
}
