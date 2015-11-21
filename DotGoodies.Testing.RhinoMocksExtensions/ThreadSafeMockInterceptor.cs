using System.Reflection;
using LinFu.DynamicProxy;

namespace DotGoodies.Testing.RhinoMocksExtensions
{
    class ThreadSafeMockInterceptor : IInterceptor
    {
        private readonly object _target;
        private readonly object _locker;

        public ThreadSafeMockInterceptor(object target, object locker)
        {
            _target = target;
            _locker = locker;
        }

        public object Intercept(InvocationInfo info)
        {
            lock (_locker)
            {
                try
                {
                    return info.TargetMethod.Invoke(_target, info.Arguments);
                }
                catch (TargetInvocationException ex)
                {
                    throw ex.FindOriginalException().PrepareToRethrow();
                }
            }
        }


    }
}

