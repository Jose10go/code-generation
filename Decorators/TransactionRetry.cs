using FancyDecoCore;
using System;
using System.Transactions;

namespace Decorators
{
    public class TransactionRetry : Decorator
    {
        private int retryTimes;
        private readonly int miliseconsDelay;

        public TransactionRetry(int retryTimes=10, int milisecondsDelay=1000)
        {
            this.retryTimes = retryTimes;
            this.miliseconsDelay = milisecondsDelay;
        }

        protected override object Decorate(Delegate d, params object[] objects)
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
}
