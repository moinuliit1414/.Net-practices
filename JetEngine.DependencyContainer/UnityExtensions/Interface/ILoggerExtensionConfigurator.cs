using JetEngine.LogEngine;
using System;
using System.Collections.Generic;
using System.Text;
using Unity.Extension;

namespace JetEngine.DependencyContainer.UnityExtensions.Interface
{
    public interface ILoggerExtensionConfigurator : IUnityContainerExtensionConfigurator
    {
        void RegisterLoggerFactory(ILoggerFactory factory);
    }
}
