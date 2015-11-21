using System;
using Rhino.Mocks.Interfaces;

namespace DotGoodies.Testing.RhinoMocksExtensions
{
    public static class MethodOptionsExtensions
    {
        public static IMethodOptions<Rhino.Mocks.RhinoMocksExtensions.VoidType> Do(
            this IMethodOptions<Rhino.Mocks.RhinoMocksExtensions.VoidType> expectation, Action action)
        {
            return expectation.Do(action);
        }

    }
}

