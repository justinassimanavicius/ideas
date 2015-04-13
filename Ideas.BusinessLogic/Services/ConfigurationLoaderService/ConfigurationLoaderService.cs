using System;
using System.Configuration;
using System.Linq;
using System.Web.Configuration;

namespace Ideas.BusinessLogic.Services.ConfigurationLoaderService
{
    /// <summary>
    /// Service used from loading configuration values from .config file.
    /// </summary>
    public class ConfigurationLoaderService : IConfigurationLoaderService
    {
        /// <summary>
        /// Retrieves specified configuration section. Recursively traverses .config file's section groups.
        /// </summary>
        /// <typeparam name="T">Configuration section type.</typeparam>
        /// <param name="group">Current configuration section group.</param>
        /// <returns>Configuration section object with config values.</returns>
        private static T TraverseConfigSections<T>(ConfigurationSectionGroup group) where T : ConfigurationSection
        {
            foreach (var section in @group.Sections.Cast<ConfigurationSection>().Where(section => Type.GetType(section.SectionInformation.Type, false) == typeof(T)))
            {
                return (T)section;
            }

            return (from ConfigurationSectionGroup g in @group.SectionGroups select TraverseConfigSections<T>(g)).FirstOrDefault(section => section != null);
        }

        /// <summary>
        /// Retrieves specified configuration section.
        /// </summary>
        /// <typeparam name="T">Configuration section type.</typeparam>
        /// <returns>Configuration section object with config values.</returns>
        private static T GetConfig<T>() where T : ConfigurationSection
        {
            T section = null;
            Configuration config = null;

            try
            {
                config = WebConfigurationManager.OpenWebConfiguration("~/Web.config");
            }
            catch { }

            if (config == null)
            {
                try
                {
                    config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
                }
                catch { }
            }

            if (config != null)
            {
                try
                {
                    section = TraverseConfigSections<T>(config.RootSectionGroup);
                }
                catch (Exception)
                {
                    //throw;
                }
            }

            if (section != null) return section;

            //checking if there are any required properties
            if (typeof(T).GetMembers().Select(member => Attribute.GetCustomAttribute(member, typeof(ConfigurationPropertyAttribute)) as ConfigurationPropertyAttribute).Any(cpAtt => (cpAtt != null) && (cpAtt.IsRequired)))
            {
                throw new ConfigurationErrorsException(String.Format("Required configuration not found: {0}", typeof(T)));
            }

            return Activator.CreateInstance<T>();
        }

        /// <inheritdoc />
        public T LoadConfig<T>() where T : ConfigurationSection
        {
            return GetConfig<T>();
        }
    }
}
