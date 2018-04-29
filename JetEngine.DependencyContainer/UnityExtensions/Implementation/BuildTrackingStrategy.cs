using System;
using System.Collections.Generic;
using System.Text;
using Unity.Builder;
using Unity.Builder.Strategy;

namespace JetEngine.DependencyContainer.UnityExtensions.Implementation
{
    public class BuildTrackingStrategy : BuilderStrategy
    {
        public override object PreBuildUp(IBuilderContext context)
        {
            //ValidationHelper.NotNull(context, "context");

            var policy = BuildTrackingPolicy.Get(context) ?? BuildTrackingPolicy.Set(context);
            //policy.Push(context.BuildKey);
            return null;
        }

        public void PostBuildUp(IBuilderContext context)
        {
            var policy = BuildTrackingPolicy.Get(context);
            if (policy != null && !policy.IsEmpty)
            {
                policy.Pop();
            }
        }
    }
}
