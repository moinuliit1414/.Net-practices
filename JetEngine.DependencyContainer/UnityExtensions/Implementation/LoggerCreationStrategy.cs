using JetEngine.DependencyContainer.UnityExtensions.Interface;
using JetEngine.LogEngine;
using System;
using System.Diagnostics;
using Unity.Builder;
using Unity.Builder.Strategy;

namespace JetEngine.DependencyContainer.UnityExtensions.Implementation
{
    public class LoggerCreationStrategy : BuilderStrategy
    {
        public void PreBuildUp(IBuilderContext context)
        {
            //ValidationHelper.NotNull(context, "context");
            string loggerName = String.Empty;
            if (context.BuildKey.Type.Equals(typeof(ILogger)))
            {
                IBuildTrackingPolicy buildTrackingPolicy = BuildTrackingPolicy.Get(context);

                if (buildTrackingPolicy != null && buildTrackingPolicy.Count >= 2)
                {
                    loggerName = buildTrackingPolicy.ElementAt(1).Type.FullName;
                }
                else
                {
                    loggerName = context.BuildKey.Name;
                }

                Debug.Assert(!String.IsNullOrEmpty(loggerName), "ILogger cannot be resolved with empty logger name. Set logger name to calling type FullName");

                //var loggerCreationPolicy = context.Policies.Get<ILoggerCreationPolicy>(NamedTypeBuildKey.Make<ILogger>());
                //context.Existing = loggerCreationPolicy.Create(loggerName);
                
            }
        }
    }
}
