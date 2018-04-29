using System;
using System.Collections.Generic;
using System.Text;

namespace JetEngine.LogEngine.Interfaces
{
    public interface IAsyncBuffer<T>
    {
        event EventHandler BufferReady;

        void Add(T item, bool force);
        IEnumerable<T> Acquire(bool force);
        void Release();
    }
}
