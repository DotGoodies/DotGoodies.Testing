using System.IO;
using log4net.Config;

namespace DotGoodies.Testing.Log4Net
{
    public sealed class LoggingTestAspect
    {
        private readonly bool _enableErrorWatch;
        private readonly string _log4NetConfigFileName;

        public LoggingTestAspect(bool enableErrorWatch = false, string log4NetConfigFileName = "log4net.config")
        {
            _enableErrorWatch = enableErrorWatch;
            _log4NetConfigFileName = log4NetConfigFileName;
        }

        public void SetUp()
        {
            XmlConfigurator.ConfigureAndWatch(new FileInfo(_log4NetConfigFileName));

            if (_enableErrorWatch)
            {
                Log4NetErrorMessagesWatch.SetUp();
            }
        }

        public Log4NetErrorMessagesWatch ErrorWatch => new Log4NetErrorMessagesWatch();

        public void TearDown()
        {
            if (_enableErrorWatch)
            {
                Log4NetErrorMessagesWatch.TearDown();
            }
        }
    }
}
