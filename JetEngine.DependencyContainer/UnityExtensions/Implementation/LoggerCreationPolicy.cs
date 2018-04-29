using JetEngine.DependencyContainer.UnityExtensions.Interface;
using JetEngine.LogEngine;
using System;
using System.Collections.Generic;
using System.Text;

namespace JetEngine.DependencyContainer.UnityExtensions.Implementation
{
    public class LoggerCreationPolicy : ILoggerCreationPolicy
    {
        private readonly ILoggerFactory _factory;

        public LoggerCreationPolicy(ILoggerFactory factory)
        {
            _factory = factory;
        }

        public ILogger Create(string name)
        {
            return _factory.Create(name);
        }
    }
}
