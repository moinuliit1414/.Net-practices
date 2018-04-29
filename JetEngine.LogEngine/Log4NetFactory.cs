using System;
using System.Collections.Generic;
using System.Text;

namespace JetEngine.LogEngine
{
    public class Log4NetFactory : ILoggerFactory
    {
        public ILogger Create(string name)
        {
            //return new Logger(name);
            return Logger.Instance;
        }
    }
}
