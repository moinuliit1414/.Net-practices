using System;
using System.Collections.Generic;
using System.Text;
using Unity.Extension;

namespace JetEngine.DependencyContainer.UnityExtensions.Implementation
{
    public class BuildTrackingExtension : UnityContainerExtension
    {
        protected override void Initialize()
        {
            //Context.Strategies.AddNew<BuildTrackingStrategy>(UnityBuildStage.TypeMapping);
        }
    }
}
