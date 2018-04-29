using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using JetEngine.DependencyContainer.UnityExtensions.Implementation;
using JetEngine.DependencyContainer.UnityExtensions.Interface;
using JetEngine.LogEngine;
using Microsoft.Practices.Unity.Configuration;
using Unity;
using Unity.Extension;
using Unity.Lifetime;
using Unity.Registration;
using Unity.Resolution;
using Microsoft.Practices.Unity;

namespace JetEngine.DependencyContainer
{
    public class DependencyResolver : IDependencyResolver, IUnityContainer
    {
        private readonly List<ExplicitLifetimeManager> _explicitRegistrations = new List<ExplicitLifetimeManager>();
        private UnityContainer _unityContainer = new UnityContainer();

        public DependencyResolver()
        {
            RegisterContainerExtensions();
        }

        #region IDisposable implementation
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                UnregisterMandatoryDependencies();

                if (_unityContainer != null)
                {
                    _unityContainer.Dispose();
                    _unityContainer = null;
                }
            }
        }
        #endregion

        [DebuggerStepThrough]
        public void LoadConfiguration()
        {
            LoadConfiguration(null);
        }

        [DebuggerStepThrough]
        public void LoadConfiguration(UnityConfigurationSection section)
        {
            if (section == null)
            {
                _unityContainer.LoadConfiguration();
            }
            else
            {
                _unityContainer.LoadConfiguration(section);
            }
            RegisterMandatoryDependencies();
        }

        #region Registration
        public IDependencyResolver Register<TFrom, TTo>()
        {
            return Register<TFrom, TTo>(null);
        }

        public IDependencyResolver Register<TFrom, TTo>(string name)
        {
            return Register<TFrom, TTo>(name, new TransientLifetimeManager());
        }

        public IDependencyResolver Register<TFrom, TTo>(string name, LifetimeManager lifetimeManager)
        {
            return RegisterType(typeof(TFrom), typeof(TTo), name, lifetimeManager);
        }

        public IDependencyResolver RegisterType(Type from, Type to, string name, LifetimeManager lifetimeManager, params InjectionMember[] injectionMembers)
        {
            _unityContainer.RegisterType(from, to, name, lifetimeManager, injectionMembers);
            return this;
        }

        IUnityContainer IUnityContainer.RegisterType(Type from, Type to, string name, LifetimeManager lifetimeManager, params InjectionMember[] injectionMembers)
        {
            return _unityContainer.RegisterType(from, to, name, lifetimeManager, injectionMembers);
        }

        public IDependencyResolver RegisterInstance<TFrom>(TFrom instance)
        {

            _unityContainer.RegisterInstance<TFrom>(instance);
            return this;
        }

        public IDependencyResolver RegisterInstance<TFrom>(TFrom instance, string name)
        {
            _unityContainer.RegisterInstance<TFrom>(name, instance);
            return this;
        }

        public IDependencyResolver RegisterInstance<TFrom>(TFrom instance, string name, LifetimeManager lifetimeManager)
        {
            _unityContainer.RegisterInstance<TFrom>(name, instance, lifetimeManager);
            return this;
        }

        IDependencyResolver IDependencyResolver.RegisterInstance(Type t, string name, object instance, LifetimeManager lifetime)
        {
            _unityContainer.RegisterInstance(t, name, instance, lifetime);
            return this;
        }

        IUnityContainer IUnityContainer.RegisterInstance(Type t, string name, object instance, LifetimeManager lifetime)
        {
            return _unityContainer.RegisterInstance(t, name, instance, lifetime);
        }
        #endregion

        public IUnityContainer Parent
        {
            get
            {
                return _unityContainer.Parent;
            }
        }

        //public IEnumerable<ContainerRegistration> Registrations
        //{
        //    get
        //    {
        //        return _unityContainer.Registrations;
        //    }
        //}

        IEnumerable<IContainerRegistration> IUnityContainer.Registrations => throw new NotImplementedException();

        public IUnityContainer AddExtension(UnityContainerExtension extension)
        {
            return _unityContainer.AddExtension(extension);
        }

        public IUnityContainer RemoveAllExtensions()
        {
            return _unityContainer.RemoveAllExtensions();
        }

        public object BuildUp(Type t, object existing, string name, params ResolverOverride[] resolverOverrides)
        {
            return _unityContainer.BuildUp(t, existing, name, resolverOverrides);
        }

        //public void Teardown(object o)
        //{
        //    _unityContainer.Teardown(o);
        //}

        public object Configure(Type configurationInterface)
        {
            return _unityContainer.Configure(configurationInterface);
        }

        public IUnityContainer CreateChildContainer()
        {
            return _unityContainer.CreateChildContainer();
        }

        #region Resolution

        [DebuggerStepThrough]
        public T Resolve<T>(params ResolverOverride[] overrides)
        {
            return _unityContainer.Resolve<T>(overrides);
        }

        [DebuggerStepThrough]
        public T Resolve<T>(string name, params ResolverOverride[] overrides)
        {
            return _unityContainer.Resolve<T>(name, overrides);
        }

        [DebuggerStepThrough]
        public object Resolve(Type t)
        {
            return _unityContainer.Resolve(t);
        }

        [DebuggerStepThrough]
        public object Resolve(Type t, string name)
        {
            return _unityContainer.Resolve(t, name);
        }

        [DebuggerStepThrough]
        public object Resolve(Type t, string name, params ResolverOverride[] resolverOverrides)
        {
            return _unityContainer.Resolve(t, name, resolverOverrides);
        }

        [DebuggerStepThrough]
        public IEnumerable<object> ResolveAll(Type t, params ResolverOverride[] resolverOverrides)
        {
            return _unityContainer.ResolveAll(t, resolverOverrides);
        }

        #endregion

        private void RegisterMandatoryDependencies()
        {
            _unityContainer.RegisterInstance<IDependencyResolver>(this, CreateExplicitLifetimeManager());
        }

        private void UnregisterMandatoryDependencies()
        {
            foreach (var registration in _explicitRegistrations)
            {
                registration.RemoveValue();
            }

            _explicitRegistrations.Clear();
        }

        private LifetimeManager CreateExplicitLifetimeManager()
        {
            var explicitManager = new ExplicitLifetimeManager();
            _explicitRegistrations.Add(explicitManager);
            return explicitManager;
        }


        private void RegisterContainerExtensions()
        {
            _unityContainer.AddNewExtension<BuildTrackingExtension>()
                           .AddNewExtension<LoggerExtension>()
                           .Configure<ILoggerExtensionConfigurator>()
                           .RegisterLoggerFactory(new Log4NetFactory());
        }

        public bool IsRegistered(Type type, string name)
        {
            throw new NotImplementedException();
        }
    }
}
