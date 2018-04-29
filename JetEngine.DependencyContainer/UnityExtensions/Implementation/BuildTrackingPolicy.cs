using JetEngine.DependencyContainer.UnityExtensions.Interface;
using System;
using System.Collections.Generic;
using System.Text;
using Unity.Builder;

namespace JetEngine.DependencyContainer.UnityExtensions.Implementation
{
    public class BuildTrackingPolicy : IBuildTrackingPolicy
    {
        private readonly Stack<NamedTypeBuildKey> _stack = new Stack<NamedTypeBuildKey>();

        public void Push(NamedTypeBuildKey t)
        {
            _stack.Push(t);
        }

        public NamedTypeBuildKey Pop()
        {
            return _stack.Pop();
        }

        public NamedTypeBuildKey ElementAt(int index)
        {
            return _stack.ToArray()[index];
        }

        public bool IsEmpty
        {
            get
            {
                return _stack.Count == 0;
            }
        }

        public int Count
        {
            get
            {
                return _stack.Count;
            }
        }

        public static IBuildTrackingPolicy Get(IBuilderContext context)
        {
            //ValidationHelper.NotNull(context, "context");

            //return context.Policies.Get<IBuildTrackingPolicy>(context.BuildKey, true);
            return null;
        }

        public static IBuildTrackingPolicy Set(IBuilderContext context)
        {
            //ValidationHelper.NotNull(context, "context");

            IBuildTrackingPolicy policy = new BuildTrackingPolicy();
            //context.Policies.SetDefault(policy);
            return policy;
        }
    }
}
