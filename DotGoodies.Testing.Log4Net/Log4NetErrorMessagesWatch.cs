using System;
using System.Collections.Concurrent;
using System.Diagnostics;
using log4net;
using log4net.Appender;
using log4net.Core;

namespace DotGoodies.Testing.Log4Net
{
    public class Log4NetErrorMessagesWatch : AppenderSkeleton
    {
        private static ConcurrentBag<Tuple<string, Exception>> _exceptions;
        private static ConcurrentBag<string> _messages;
        private static readonly string SelfTestUniqueMessage = $"Self test {Guid.NewGuid()}.";

        private static bool _selfTestPassed = false;

        internal static void SetUp()
        {
            _exceptions = new ConcurrentBag<Tuple<string, Exception>>();
            _messages = new ConcurrentBag<string>();

            var testLogger = LogManager.GetLogger(typeof(Log4NetErrorMessagesWatch));

            testLogger.ErrorFormat(SelfTestUniqueMessage);

            if (!_selfTestPassed)
                throw new InvalidOperationException("Log4NetErrorMessagesWatch is not properly configured.");
        }

        internal static void TearDown()
        {
            _selfTestPassed = false;

            _exceptions = new ConcurrentBag<Tuple<string, Exception>>();
            _messages = new ConcurrentBag<string>();
        }

        protected override void Append(LoggingEvent loggingEvent)
        {
            if (loggingEvent.RenderedMessage.Equals(SelfTestUniqueMessage))
            {
                _selfTestPassed = true;
                return;
            }

            if (loggingEvent.Level != Level.Error && loggingEvent.Level != Level.Fatal)
                return;

            if (loggingEvent.ExceptionObject != null)
            {
                _exceptions.Add(Tuple.Create(loggingEvent.RenderedMessage, loggingEvent.ExceptionObject));
                return;
            }

            _messages.Add(loggingEvent.RenderedMessage);
        }

        public void PrintExceptions()
        {
            if (_exceptions.Count == 0 && _messages.Count == 0)
                return;

            if (_exceptions.Count > 0)
                Debug.WriteLine("Exceptions:");

            foreach (var exception in _exceptions)
            {
                Debug.WriteLine("Message: {0}. Exception: {1}", exception.Item1, exception.Item2);
            }

            if (_messages.Count > 0)
                Debug.WriteLine("Messages:");

            foreach (var message in _messages)
            {
                Debug.WriteLine(message);
            }
        }

        public bool HasExceptions => _exceptions.Count > 0 || _messages.Count > 0;
    }
}
