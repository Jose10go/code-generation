using System;

namespace Tests.Examples.ReplaceInvocation
{
    public class A 
    {
        public void hello()
        {
            this.f((i)=>i.ToString());
            f((i)=>(i*i).ToString());
        }

        public void f(Func<int,string> exp) 
        {
            throw new Exception();
        }

        public void f(string code)
        {
            Console.WriteLine(code);
        }
    }
}
