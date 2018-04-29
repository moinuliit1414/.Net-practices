using JetEngine.LogEngine;
using System;
using System.Collections.Generic;
using System.Text;
using Unity.Policy;

namespace JetEngine.DependencyContainer.UnityExtensions.Interface
{
    public interface ILoggerCreationPolicy : IBuilderPolicy
    {
        ILogger Create(string name);
    }
}
