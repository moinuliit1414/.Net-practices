using System;
using System.Collections.Generic;
using System.Text;
using Unity.Builder;
using Unity.Policy;

namespace JetEngine.DependencyContainer.UnityExtensions.Interface
{
    public interface IBuildTrackingPolicy : IBuilderPolicy
    {
        void Push(NamedTypeBuildKey t);
        NamedTypeBuildKey Pop();
        NamedTypeBuildKey ElementAt(int index);

        bool IsEmpty { get; }

        int Count { get; }
    }
}
