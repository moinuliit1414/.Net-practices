using Microsoft.Practices.Unity.Configuration;
using System;
using Unity.Lifetime;
using Unity.Registration;
using Unity.Resolution;

namespace JetEngine.DependencyContainer
{
    public interface IDependencyResolver : IDisposable
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter")]
        IDependencyResolver Register<TForm, TTo>();
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter")]
        IDependencyResolver Register<TFrom, TTo>(string name);
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter")]
        IDependencyResolver Register<TFrom, TTo>(string name, LifetimeManager lifetimeManager);
        IDependencyResolver RegisterInstance<TFrom>(TFrom instance);
        IDependencyResolver RegisterInstance<TFrom>(TFrom instance, string name);
        IDependencyResolver RegisterInstance<TFrom>(TFrom instance, string name, LifetimeManager lifetimeManager);

        void LoadConfiguration();

        void LoadConfiguration(UnityConfigurationSection section);

        T Resolve<T>(params ResolverOverride[] overrides);
        T Resolve<T>(string name, params ResolverOverride[] overrides);

        object Resolve(Type t);
        object Resolve(Type t, string name);

        IDependencyResolver RegisterType(Type from, Type to, string name, LifetimeManager lifetimeManager, params InjectionMember[] injectionMembers);
        IDependencyResolver RegisterInstance(Type t, string name, object instance, LifetimeManager lifetime);
        object Resolve(Type t, string name, params ResolverOverride[] resolverOverrides);
    }
}
