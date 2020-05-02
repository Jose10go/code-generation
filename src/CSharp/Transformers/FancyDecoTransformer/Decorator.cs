using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Transactions;

namespace FancyDecoTransformer
{
    [AttributeUsage(AttributeTargets.Method,AllowMultiple =true,Inherited=true)]
    public abstract class DecoratorAttribute:Attribute
    {
        protected virtual object Decorator(Delegate d,params object[] objects)
        {
            return d.DynamicInvoke(objects);
        }
        
        public Func<TResult> Decorate<TResult>(Func<TResult> f)
        {
            return () => (TResult)Decorator(f);
        }

        public Func<T1,TResult> Decorate<T1,TResult>(Func<T1,TResult> f)
        {
            return (arg1) => (TResult)Decorator(f,arg1);
        }

        public Func<T1,T2,TResult> Decorate<T1,T2,TResult>(Func<T1,T2,TResult> f)
        {
            return (arg1,arg2) => (TResult)Decorator(f,arg1,arg2);
        }

        public Func<T1,T2,T3,TResult> Decorate<T1,T2,T3,TResult>(Func<T1,T2,T3,TResult> f)
        {
            return (arg1,arg2,arg3) => (TResult)Decorator(f, arg1,arg2,arg3);
        }

        public Action<T1> Decorate<T1>(Action<T1> f)
        {
            return (arg1) => Decorator(f, arg1);
        }

        public Action<T1,T2> Decorate<T1, T2>(Action<T1,T2> f)
        {
            return (arg1, arg2) => Decorator(f, arg1, arg2);
        }

        public Action<T1,T2,T3> Decorate<T1,T2,T3>(Action<T1,T2,T3> f)
        {
            return (arg1, arg2, arg3) => Decorator(f, arg1, arg2, arg3);
        }
    }

    public class MemoizeAttribute : DecoratorAttribute
    {
        private class Comparer : IEqualityComparer<object[]>
        {
            public bool Equals([AllowNull] object[] x, [AllowNull] object[] y)
            {
                if (x is null && y is null)
                    return true;
                if (x is null || y is null)
                    return false;
                if(x.Length!=y.Length)
                    return false;
                for (int i = 0; i < x.Length; i++)
                    if (!x[i]?.Equals(y[i])??y[i]==null)
                        return false;
                return true;
            }

            public int GetHashCode([DisallowNull] object[] obj)
            {
                return obj?.Sum(x => x?.GetHashCode()??0)??0;
            }
        }

        readonly Dictionary<object[], object> dictionary = new Dictionary<object[], object>(new Comparer());

        protected override object Decorator(Delegate d, params object[] objects)
        {
            if (dictionary.ContainsKey(objects))
                return dictionary[objects];
            return d.DynamicInvoke(objects);
        }

    }

    public class CronoAttribute : DecoratorAttribute 
    {
        protected override object Decorator(Delegate d, params object[] objects)
        {
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            var result=d.DynamicInvoke(objects);
            stopwatch.Stop();
            Console.WriteLine($"Method {d.Method.Name} run in {stopwatch.ElapsedMilliseconds}");
            return result;
        }
    }

    public class ThrowOnNullAttribute : DecoratorAttribute
    {
        protected override object Decorator(Delegate d, params object[] objects)
        {
            var parameters = d.Method.GetParameters();
            for (int i = 0; i < objects.Length; i++)
                if (objects[i] is null)
                    throw new ArgumentNullException($"Check that parameter {parameters[i].Name} is not null.");
            return d.DynamicInvoke(objects);
        }
    }

    public class TransactionRetryAttribute : DecoratorAttribute
    {
        private int retryTimes;
        private readonly int miliseconsDelay;

        public TransactionRetryAttribute(int retryTimes,int milisecondsDelay)
        {
            this.retryTimes = retryTimes;
            this.miliseconsDelay = milisecondsDelay;
        }

        protected override object Decorator(Delegate d, params object[] objects)
        {
            using (var scope = new TransactionScope())
            {
                while (true)
                {
                    try
                    {
                        var res = d.DynamicInvoke(objects);
                        scope.Complete();
                        return res;
                    }
                    catch
                    {
                        if (retryTimes >= 0)
                        {
                            retryTimes--;
                            Console.WriteLine($"Left {retryTimes} times...");
                            Console.WriteLine($"Waiting {this.miliseconsDelay} miliseconds to retry...");
                            System.Threading.Tasks.Task.Delay(this.miliseconsDelay);
                        }
                        else
                            throw;
                    }
                }
            }
        }
    }

    public class TagsCompositionAttribute : DecoratorAttribute
    {
        readonly string tag;
        public TagsCompositionAttribute(string tag)
        {
            this.tag = tag;
        }
        
        protected override object Decorator(Delegate d,params object[] objects)
        {
            return "<" + tag + ">\n" + d.DynamicInvoke(objects)?.ToString()??"" + " \n<" + tag + "/>";
        }

    }

    public class LoggingAttribute : DecoratorAttribute
    {
        protected override object Decorator(Delegate d,params object[] objects)
        {
            Console.WriteLine($"Starting execution of {d.Method.Name} with arguments {String.Join(',',objects.Select(x=>x?.ToString()??"<null>"))}.");
            var res = d.DynamicInvoke(objects);
            Console.WriteLine($"Ending execution of {d.Method.Name} with arguments {String.Join(',',objects.Select(x=>x?.ToString()??"<null>"))}.");
            return res;
        }
    }
}
