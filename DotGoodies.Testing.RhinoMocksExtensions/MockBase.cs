using System.Collections.Generic;
using System.Linq;
using LinFu.DynamicProxy;
using Rhino.Mocks;

namespace DotGoodies.Testing.RhinoMocksExtensions
{
    public abstract class MockBase
    {
        private bool _skipVerification;

        private readonly ProxyFactory _threadSafeProxyFactory = new ProxyFactory();
        private readonly object _syncLock = new object();

        protected MockRepository Mocks { get; private set; }

        protected void MockBaseTestSetUp()
        {
            _skipVerification = false;
            Mocks = new MockRepository();
        }

        protected void Replay()
        {
            Mocks.ReplayAll();
        }

        protected void SkipVerification()
        {
            _skipVerification = true;
        }

        protected void BackToRecord()
        {
            Mocks.BackToRecordAll();
        }

        //TODO [IF] - this nasty workaround was done because Rhino Mocks has deadlock problem when same mock is accessed concurrently.
        //TODO [IF] - that makes this mocking framework not ideal. Seems that we'll have to find a better one.
        //TODO [IF] - communicate with Ayende or Mike Meisinger - maybe they can help fixing it.
        //TODO [IF] - a topic started here. https://groups.google.com/forum/#!topic/rhinomocks/InFD86drbwk
        protected T ThreadSafe<T>(T mock)
        {
            return _threadSafeProxyFactory.CreateProxy<T>(
                new ThreadSafeMockInterceptor(mock, _syncLock));
        }

        protected T Mock<T>() where T : class
        {
            return Mocks.StrictMock<T>();
        }

        protected List<T> MakeMocks<T>(int count) where T : class
        {
            var result = new List<T>();

            for (int i = 0; i < count; i++)
            {
                result.Add(Mock<T>());
            }

            return result;
        }

        protected static T Stub<T>() where T : class
        {
            return MockRepository.GenerateStub<T>();
        }

        protected List<TMain> MakeMultiMocks<TMain, TAdditional>(int count)
        {
            var result = new List<TMain>();

            for (int i = 0; i < count; i++)
            {
                result.Add(Mocks.StrictMultiMock<TMain>(typeof(TAdditional)));
            }

            return result;
        }

        protected TMain MultiMock<TMain, TAdditional>()
        {
            return Mocks.StrictMultiMock<TMain>(typeof(TAdditional));
        }

        protected TMain MultiMock<TMain, TAdditional1, TAdditional2>()
        {
            return Mocks.StrictMultiMock<TMain>(typeof(TAdditional1), typeof(TAdditional2));
        }

        protected List<T> List<T>(params T[] items)
        {
            return items.ToList();
        } 

        protected void MockBaseTearDown()
        {
            if (!_skipVerification)
                Mocks.VerifyAll();
        }
    }
}

