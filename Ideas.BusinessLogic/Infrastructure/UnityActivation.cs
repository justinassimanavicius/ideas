using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ideas.BusinessLogic.Factories;
using Microsoft.Practices.Unity;

namespace Ideas.BusinessLogic.Infrastructure
{
    public static class UnityActivation
    {
        public static IUnityContainer Container { get; private set; }

        public static void Activate()
        {
            Container = ServiceContainerFactory.CreateUnityContainer();
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
