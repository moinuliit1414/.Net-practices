using JetEngine.LogEngine.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace JetEngine.LogEngine.Interfaces
{
    public interface IGrowLimiter
    {
        event EventHandler<ErrorEventArgs> Error;

        bool CheckLimitReached(int totalSize);
    }
}