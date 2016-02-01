using System;
using Ideas.BusinessLogic.Factories;
using IdeasAPI.Helpers;
using Microsoft.Practices.Unity;

namespace IdeasAPI.Infrastructure
{
    public static class UnityActivation
    {
        public static IUnityContainer Container { get; private set; }

        public static void Activate()
        {
            Container = ServiceContainerFactory.CreateUnityContainer();

            Container.RegisterType<IUserRepository, UserRepository>(new ContainerControlledLifetimeManager());

#if DEBUG
            Container.RegisterType<IUserRepository, FakeUserRepository>(new ContainerControlledLifetimeManager());
#endif
        }

        public static object Resolve(Type typeToResolve)
        {
            return Container.Resolve(typeToResolve);
        }

        public static object Resolve(Type typeToResolve, string instanceName)
        {
            return Container.Resolve(typeToResolve, instanceName);
        }

        /// <summary>
        /// Generic shorthand for Container.Resolve
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static T Resolve<T>()
        {
            return Container.Resolve<T>();
        }
    }
}
