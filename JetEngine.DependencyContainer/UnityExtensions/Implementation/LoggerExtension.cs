using JetEngine.DependencyContainer.UnityExtensions.Interface;
using JetEngine.LogEngine;
using System;
using System.Linq;
using Unity.Builder;
using Unity.Extension;
namespace JetEngine.DependencyContainer.UnityExtensions.Implementation
{
    public class LoggerExtension : UnityContainerExtension, ILoggerExtensionConfigurator
    {
        public void RegisterLoggerFactory(ILoggerFactory factory)
        {
            //Context.Policies.Set<ILoggerCreationPolicy>(new LoggerCreationPolicy(factory), NamedTypeBuildKey.Make<ILogger>());
        }

        protected override void Initialize()
        {
            //Context.Strategies.AddNew<LoggerCreationStrategy>(UnityBuildStage.PreCreation);
        }
    }
}
