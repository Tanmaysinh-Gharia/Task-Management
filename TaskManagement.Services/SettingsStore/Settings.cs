using Microsoft.Extensions.Configuration;


namespace TaskManagement.Services.SettingsStore
{

    /// <summary>
    /// Centralized static settings store
    /// </summary>
    public class Settings
    {
        private static readonly Dictionary<string, string> ConfigurationDetails = new();


        /// <summary>
        /// Gets or sets the base API URL used across services. Backed by the configuration dictionary.
        /// </summary>
        public static string ApiUrl
        {
            get => ConfigurationDetails["ApiSettings:BaseUrl"];
            set => ConfigurationDetails["ApiSettings:BaseUrl"] = value;
        }

        /// <summary>
        /// Gets or sets the default page size used in pagination. Backed by the configuration dictionary.
        /// </summary>
        public static int PageSize
        {
            get => Convert.ToInt32(ConfigurationDetails["Pagination:DefaultPageSize"]);
            set => ConfigurationDetails["Pagination:DefaultPageSize"] = value.ToString();
        }


        /// <summary>
        /// Loads API base URL and pagination settings from configuration and stores them in-memory.
        /// Should be called during application startup.
        /// </summary>
        public static void LoadFromConfiguration(IConfiguration config)
        {
            var section = config.GetSection("ApiSettings");
            ConfigurationDetails["ApiSettings:BaseUrl"] = section["BaseUrl"]!;
            var pageSection = config.GetSection("Pagination");
            ConfigurationDetails["Pagination:DefaultPageSize"] = pageSection["DefaultPageSize"]!;
        }
    }
}
