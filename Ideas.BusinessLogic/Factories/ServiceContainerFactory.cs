using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using Microsoft.Practices.Unity;
using Microsoft.Practices.Unity.Configuration;
using Microsoft.Practices.Unity.InterceptionExtension;

namespace Ideas.BusinessLogic.Factories
{
    public static class ServiceContainerFactory
    {
        public static IUnityContainer CreateUnityContainer()
        {
            return CreateUnityContainer(false);
        }

        public static IUnityContainer CreateUnityContainer(bool ignoreOverrides)
        {
            IUnityContainer container = new UnityContainer();
            container.AddNewExtension<Interception>();

            var unitySection = (UnityConfigurationSection)ConfigurationManager.GetSection("unity");
            unitySection.Configure(container, "common");

            if (!ignoreOverrides)
            {
                foreach (string containerOverride in GetUnityContainerOverrides())
                {
                    if (unitySection.Containers.Any(c => string.Equals(c.Name, containerOverride, System.StringComparison.OrdinalIgnoreCase)))
                    {
                        container = container.CreateChildContainer();
                        unitySection.Configure(container, containerOverride);
                    }
                }
            }

            return container;
        }

        private static IEnumerable<string> GetUnityContainerOverrides()
        {
            string val = ConfigurationManager.AppSettings["unityContainerOverrides"];
            if (!string.IsNullOrEmpty(val))
            {
                return val.Split(',').Select(v => v.Trim());
            }

            return Enumerable.Empty<string>();
        }
    }
}
