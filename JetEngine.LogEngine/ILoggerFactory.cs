using System;
using System.Collections.Generic;
using System.Text;

namespace JetEngine.LogEngine
{
    public interface ILoggerFactory
    {
        ILogger Create(string name);
    }
}
