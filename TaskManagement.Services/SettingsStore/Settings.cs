using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace TaskManagement.Services.SettingsStore
{

    /// <summary>
    /// Centralized static settings store
    /// </summary>
    public class Settings
    {
        private static readonly Dictionary<string, string> ConfigurationDetails = new();

        public static string ApiUrl
        {
            get => ConfigurationDetails["ApiSettings:BaseUrl"];
            set => ConfigurationDetails["ApiSettings:BaseUrl"] = value;
        }

        public static int PageSize
        {
            get => Convert.ToInt32(ConfigurationDetails["Pagination:DefaultPageSize"]);
            set => ConfigurationDetails["Pagination:DefaultPageSize"] = value.ToString();
        }

        public static void LoadFromConfiguration(IConfiguration config)
        {
            var section = config.GetSection("ApiSettings");
            ConfigurationDetails["ApiSettings:BaseUrl"] = section["BaseUrl"]!;
            var pageSection = config.GetSection("Pagination");
            ConfigurationDetails["Pagination:DefaultPageSize"] = pageSection["DefaultPageSize"]!;
            // Add more entries here if needed
        }
    }
}
