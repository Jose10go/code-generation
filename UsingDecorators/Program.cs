using System;
using System.Data;
using Decorators;

namespace UsingDecorators
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Write your name:");
            GetYourName();
            Console.WriteLine();
            Console.WriteLine("Write your Age:");
            GetYourAge();
            Console.WriteLine();
            Console.WriteLine("What fibbonaci you like the most?");
            var n = int.Parse(Console.ReadLine());
            Console.WriteLine("For the first Time:");
            Console.WriteLine("Your fib number is "+RecursiveFibb(n));
            Console.WriteLine();
            Console.WriteLine("For the second Time:");
            Console.WriteLine("Your fib number is " + RecursiveFibb(n));
            Console.WriteLine("Wow!!!that was fast...");
            Console.WriteLine();
            Console.WriteLine("Your Random Even Number is " + RandomEvenNumber());
            Console.WriteLine();
            Console.ReadLine();
        }

        [Crono]
        static string GetYourName()
        {
            return Console.ReadLine();
        }

        [Logg]
        [Crono]
        static int GetYourAge()
        {
            return int.Parse(Console.ReadLine());
        }

        [Crono, Memoize]
        static int RecursiveFibb(int n) 
            => n < 2 ? 1 : RecursiveFibb(n - 1) + RecursiveFibb(n - 2);
        
        [Crono]
        [TransactionRetry]
        static int RandomEvenNumber() 
        {
            Random r = new Random();
            var n=r.Next(6);
            if (n % 2 == 0)
                return n;
            throw new InvalidConstraintException("Why Odd... i like even...");
        }
    }
}
