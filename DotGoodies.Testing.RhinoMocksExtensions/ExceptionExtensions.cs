using System;
using System.Reflection;

namespace DotGoodies.Testing.RhinoMocksExtensions
{
    //TODO [IF] - this seems to be more general. Revisit.
    internal static class ExceptionExtensions
    {
        public static T PrepareToRethrow<T>(this T ex) where T : Exception
        {
            typeof(Exception).GetMethod("PrepForRemoting",
                BindingFlags.NonPublic | BindingFlags.Instance)
                .Invoke(ex, new object[0]);
            return ex;
        }

        public static Exception FindOriginalException(this TargetInvocationException exception)
        {
            Exception currentException = exception;
            while (true)
            {
                if (currentException.InnerException == null) { break; }
                currentException = currentException.InnerException;

                if (!(currentException is TargetInvocationException)) { break; }
            }

            return currentException;
        }

    }
}
