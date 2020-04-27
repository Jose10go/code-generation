using System;
namespace ExtensionMethod
{
    public static class Extensions<T1>
    {
        public static T2 ExtensionMethod<T2>(this T1 self,Func<T1,T2> func,string message="hello")
            where T2:T1
        { 
            Console.WriteLine(message);
            return func(self);
        }
    }
}