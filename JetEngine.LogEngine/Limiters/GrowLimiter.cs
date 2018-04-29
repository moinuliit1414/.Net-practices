using JetEngine.LogEngine.Common;
using JetEngine.LogEngine.Interfaces;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace JetEngine.LogEngine.Limiters
{
    public class GrowLimiter : IGrowLimiter
    {
        #region Fields

        private readonly GrowType _growType;
        private readonly int _growLimit;
        private bool _isOverflow;

        #endregion Fields


        public GrowLimiter(GrowType growType, int growLimit)
        {
            _growType = growType;
            _growLimit = growLimit;
        }


        #region IGrowLimiter

        public event EventHandler<ErrorEventArgs> Error;

        public bool CheckLimitReached(int totalSize)
        {
            if (_growType == GrowType.Grow)
            {
                return false;
            }
            if (_growType == GrowType.Skip && totalSize >= _growLimit)
            {
                return true;
            }
            if (_growType != GrowType.Error)
            {
                return false;
            }
            // Reset overflow flag after threshold
            if (_isOverflow && totalSize <= (_growLimit / 2))
            {
                _isOverflow = false;
            }
            if (totalSize < _growLimit)
            {
                // limit is ok
                return false;
            }
            if (_isOverflow)
            {
                // error already reported
                return true;
            }
            OnError(
                "Logging buffer overflow ({0} events). Some logging events were skipped!",
                _growLimit);
            _isOverflow = true;
            return true;
        }

        #endregion IGrowLimiter


        #region Private

        private void OnError(string fmt, params object[] args)
        {
            var handler = Error;
            if (handler != null)
            {
                var arg = new ErrorEventArgs(
                    string.Format(CultureInfo.InvariantCulture, fmt, args));
                handler(this, arg);
            }
        }

        #endregion Private
    }
}
