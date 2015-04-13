using System.Configuration;

namespace Ideas.BusinessLogic.Services.ConfigurationLoaderService
{
    /// <summary>
    /// Service used from loading configuration values from .config file.
    /// </summary>
    public interface IConfigurationLoaderService
    {
        /// <summary>
        /// Loads specified configuration section configuration values.
        /// </summary>
        /// <typeparam name="T">Configuration section type.</typeparam>
        /// <returns>Configuration section object with config values.</returns>
        T LoadConfig<T>() where T : ConfigurationSection;
    }
}