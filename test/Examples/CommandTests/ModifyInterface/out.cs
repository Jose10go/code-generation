namespace Tests.Examples.ModifyInterface
{
    internal interface I_modified<T>
        where T : class,new()
    {
        void goodBye();
        void hello();
    }
}
