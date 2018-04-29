using System;
using System.Collections.Generic;
using System.Text;

namespace JetEngine.LogEngine.Common
{
    public class ErrorEventArgs : EventArgs
    {
        public string Text { get; private set; }

        public ErrorEventArgs(string text)
        {
            Text = text;
        }
    }
}
