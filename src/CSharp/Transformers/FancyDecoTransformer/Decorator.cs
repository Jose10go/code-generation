using System;

namespace FancyDecoTransformer
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = true, Inherited = true)]
    public abstract class DecoratorAttribute : Attribute 
    {
    }
    
    public abstract class Decorator
    {
        protected abstract object Decorate(Delegate d, params object[] objects);

        public Func<TResult> Decorate<TResult>(Func<TResult> f)
        {
            return () => (TResult)Decorate((Delegate)f);
        }

        public Func<T1,TResult> Decorate<T1,TResult>(Func<T1,TResult> f)
        {
            return (arg1) => (TResult)Decorate(f,arg1);
        }

        public Func<T1,T2,TResult> Decorate<T1,T2,TResult>(Func<T1,T2,TResult> f)
        {
            return (arg1,arg2) => (TResult)Decorate(f,arg1,arg2);
        }

        public Func<T1,T2,T3,TResult> Decorate<T1,T2,T3,TResult>(Func<T1,T2,T3,TResult> f)
        {
            return (arg1,arg2,arg3) => (TResult)Decorate(f, arg1,arg2,arg3);
        }

        public Action Decorate(Action f)
        {
            return () => Decorate((Delegate)f);
        }

        public Action<T1> Decorate<T1>(Action<T1> f)
        {
            return (arg1) => Decorate(f, arg1);
        }

        public Action<T1,T2> Decorate<T1, T2>(Action<T1,T2> f)
        {
            return (arg1, arg2) => Decorate(f, arg1, arg2);
        }

        public Action<T1,T2,T3> Decorate<T1,T2,T3>(Action<T1,T2,T3> f)
        {
            return (arg1, arg2, arg3) => Decorate(f, arg1, arg2, arg3);
        }
    }

    public abstract class Decorator<G1>
    {
        protected abstract object Decorate(Delegate d, G1 arg0, params object[] objects);

        public Func<T1, TResult> Decorate<T1, TResult>(Func<T1, TResult> f)
            where T1:G1
        {
            return (arg1) => (TResult)Decorate(f, arg1);
        }

        public Func<T1, T2, TResult> Decorate<T1, T2, TResult>(Func<T1, T2, TResult> f)
            where T1:G1
        {
            return (arg1, arg2) => (TResult)Decorate(f, arg1, arg2);
        }

        public Func<T1, T2, T3, TResult> Decorate<T1, T2, T3, TResult>(Func<T1, T2, T3, TResult> f)
            where T1:G1
        {
            return (arg1, arg2, arg3) => (TResult)Decorate(f, arg1, arg2, arg3);
        }

        public Action<T1> Decorate<T1>(Action<T1> f)
            where T1:G1
        {

            return (arg1) => Decorate(f, arg1);
        }

        public Action<T1, T2> Decorate<T1, T2>(Action<T1, T2> f)
            where T1:G1
        {
            return (arg1, arg2) => Decorate(f, arg1, arg2);
        }

        public Action<T1, T2, T3> Decorate<T1, T2, T3>(Action<T1, T2, T3> f)
            where T1:G1
        {
            return (arg1, arg2, arg3) => Decorate(f, arg1, arg2, arg3);
        }
    }

    public abstract class Decorator<G1,G2>
    {
        protected abstract object Decorate(Delegate d, G1 arg0, G2 arg1, params object[] objects);

        public Func<T1, T2, TResult> Decorate<T1, T2, TResult>(Func<T1, T2, TResult> f)
            where T1 : G1
            where T2 : G2
        {
            return (arg1, arg2) => (TResult)Decorate(f, arg1, arg2);
        }

        public Func<T1, T2, T3, TResult> Decorate<T1, T2, T3, TResult>(Func<T1, T2, T3, TResult> f)
            where T1 : G1
            where T2 : G2
        {
            return (arg1, arg2, arg3) => (TResult)Decorate(f, arg1, arg2, arg3);
        }

        public Action<T1, T2> Decorate<T1, T2>(Action<T1, T2> f)
            where T1 : G1
            where T2 : G2
        {
            return (arg1, arg2) => Decorate(f, arg1, arg2);
        }

        public Action<T1, T2, T3> Decorate<T1, T2, T3>(Action<T1, T2, T3> f)
            where T1 : G1
            where T2 : G2
        {
            return (arg1, arg2, arg3) => Decorate(f, arg1, arg2, arg3);
        }
    }

    public abstract class Decorator<G1,G2,G3>
    {
        protected abstract object Decorate(Delegate d, G1 arg0, G2 arg1, G3 arg2, params object[] objects);

        public Func<T1, T2, T3, TResult> Decorate<T1, T2, T3, TResult>(Func<T1, T2, T3, TResult> f)
            where T1 : G1
            where T2 : G2
            where T3 : G3
        {
            return (arg1, arg2, arg3) => (TResult)Decorate(f, arg1, arg2, arg3);
        }

        public Action<T1, T2, T3> Decorate<T1, T2, T3>(Action<T1, T2, T3> f)
            where T1 : G1
            where T2 : G2
            where T3 : G3
        {
            return (arg1, arg2, arg3) => Decorate(f, arg1, arg2, arg3);
        }
    }

    public abstract class FuncDecorator<GResult>
    {
        protected abstract GResult Decorate(Delegate d, params object[] objects);

        public Func<TResult> Decorate<TResult>(Func<TResult> f)
            where TResult:GResult
        {
            return () => (TResult)Decorate((Delegate)f);
        }

        public Func<T1, TResult> Decorate<T1, TResult>(Func<T1, TResult> f)
            where TResult:GResult
        {
            return (arg1) => (TResult)Decorate(f, arg1);
        }

        public Func<T1, T2, TResult> Decorate<T1, T2, TResult>(Func<T1, T2, TResult> f)
            where TResult:GResult
        {
            return (arg1, arg2) => (TResult)Decorate(f, arg1, arg2);
        }

        public Func<T1, T2, T3, TResult> Decorate<T1, T2, T3, TResult>(Func<T1, T2, T3, TResult> f)
            where TResult:GResult
        {
            return (arg1, arg2, arg3) => (TResult)Decorate(f, arg1, arg2, arg3);
        }

    }

    public abstract class FuncDecorator<GResult,G1>
    {
        protected abstract GResult Decorate(Delegate d, G1 arg0,params object[] objects);

        public Func<T1, TResult> Decorate<T1, TResult>(Func<T1, TResult> f)
            where TResult : GResult
            where T1:G1
        {
            return (arg1) => (TResult)Decorate(f, arg1);
        }

        public Func<T1, T2, TResult> Decorate<T1, T2, TResult>(Func<T1, T2, TResult> f)
            where TResult : GResult
            where T1:G1
        {
            return (arg1, arg2) => (TResult)Decorate(f, arg1, arg2);
        }

        public Func<T1, T2, T3, TResult> Decorate<T1, T2, T3, TResult>(Func<T1, T2, T3, TResult> f)
            where TResult : GResult
            where T1:G1
        {
            return (arg1, arg2, arg3) => (TResult)Decorate(f, arg1, arg2, arg3);
        }
    }

    public abstract class FuncDecorator<GResult, G1, G2>
    {
        protected abstract GResult Decorate(Delegate d, G1 arg0,G2 arg1,params object[] objects);

        public Func<T1, T2, TResult> Decorate<T1, T2, TResult>(Func<T1, T2, TResult> f)
            where TResult : GResult
            where T1 : G1
            where T2 : G2
        {
            return (arg1, arg2) => (TResult)Decorate(f,arg1, arg2);
        }

        public Func<T1, T2, T3, TResult> Decorate<T1, T2, T3, TResult>(Func<T1, T2, T3, TResult> f)
            where TResult : GResult
            where T1 : G1
            where T2 : G2
        {
            return (arg1, arg2, arg3) => (TResult)Decorate(f, arg1, arg2, arg3);
        }
    }

    public abstract class FuncDecorator<GResult, G1, G2, G3>
    {
        protected abstract GResult Decorate(Delegate d, G1 arg0,G2 arg1,G3 arg2, params object[] objects);

        public Func<T1, T2, T3, TResult> Decorate<T1, T2, T3, TResult>(Func<T1, T2, T3, TResult> f)
            where TResult : GResult
            where T1 : G1
            where T2 : G2
            where T3 : G3
        {
            return (arg1, arg2, arg3) => (TResult)Decorate(f, arg1, arg2, arg3);
        }
    }

    public abstract class ActionDecorator
    {
        protected abstract void Decorate(Delegate d, params object[] objects);

        public Action Decorate(Action f)
        {
            return () => Decorate((Delegate)f);
        }

        public Action<T1> Decorate<T1>(Action<T1> f)
        {
            return (arg1) => Decorate((Delegate)f, arg1);
        }

        public Action<T1, T2> Decorate<T1, T2>(Action<T1, T2> f)
        {
            return (arg1, arg2) => Decorate(f, arg1, arg2);
        }

        public Action<T1, T2, T3> Decorate<T1, T2, T3>(Action<T1, T2, T3> f)
        {
            return (arg1, arg2, arg3) => Decorate(f, arg1, arg2, arg3);
        }

    }

    public abstract class ActionDecorator<G1>
    {
        protected abstract void Decorate(Delegate d, G1 arg0, params object[] objects);

        public Action<T1> Decorate<T1>(Action<T1> f)
            where T1:G1
        {
            return (arg1) => Decorate(f, arg1);
        }

        public Action<T1, T2> Decorate<T1, T2>(Action<T1, T2> f)
            where T1:G1
        {
            return (arg1, arg2) => Decorate(f, arg1, arg2);
        }

        public Action<T1, T2, T3> Decorate<T1, T2, T3>(Action<T1, T2, T3> f)
            where T1:G1
        {
            return (arg1, arg2, arg3) => Decorate(f, arg1, arg2, arg3);
        }
    }

    public abstract class ActionDecorator<G1,G2>
    {
        protected abstract void Decorate(Delegate d, G1 arg0,G2 arg1,params object[] objects);

        public Action<T1,T2> Decorate<T1,T2>(Action<T1,T2> f)
            where T1 : G1
            where T2 : G2
        {
            return (arg1,arg2) => Decorate(f, arg1,arg2);
        }
        
        public Action<T1, T2, T3> Decorate<T1, T2, T3>(Action<T1, T2, T3> f)
            where T1 : G1
            where T2 : G2
        {
            return (arg1, arg2, arg3) => Decorate(f, arg1, arg2, arg3);
        }
    }

    public abstract class ActionDecorator<G1, G2, G3>
    {
        protected abstract void Decorate(Delegate d, G1 arg0, G2 arg1,G3 arg2, params object[] objects);

        public Action<T1, T2, T3> Decorate<T1, T2, T3>(Action<T1, T2, T3> f)
            where T1 : G1
            where T2 : G2
            where T3 : G3
        {
            return (arg1, arg2, arg3) => Decorate(f, arg1, arg2, arg3);
        }
    }
}

