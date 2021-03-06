using Microsoft.Extensions.Configuration;

namespace Infrastructure.Services
{
    public static class AppSettings
    {
        private static IConfiguration _config;

        public static void Initialize(IConfiguration configuration)
        {
            _config = configuration;
        }

        public static string DefaultConnection { get { return _config.GetConnectionString("DefaultConnection"); } }
        public static string GetProperty(string name) => _config[name];
        public static string GetAppConstant(string name) => _config[$"AppConstants:{name}"];
    }
}
