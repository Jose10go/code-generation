using System;
using Decorators;
namespace UsingDecorators
{
    static class Program
    {
        static void Main(string[] args)
        {
            F();
        }

        [Crono]
        static void F() 
        {
            System.Threading.Thread.Sleep(10000);
            Console.ReadLine();
        }
    }
}
