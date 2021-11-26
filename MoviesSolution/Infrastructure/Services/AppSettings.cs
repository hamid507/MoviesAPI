using Microsoft.Extensions.Configuration;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Infrastructure.Services
{
    public static class AppSettings
    {
        private static IConfiguration _config;

        public static void Initialize(IConfiguration configuration)
        {
            _config = configuration;
        }

        public static string DebugConnection { get { return _config.GetConnectionString("DebugConnection"); } }
        public static string ReleaseConnection { get { return _config.GetConnectionString("ReleaseConnection"); } }
        public static string MigrationConnection { get { return _config.GetConnectionString("MigrationConnection"); } }
        public static string GetProperty(string name) => _config[name];
        public static string GetAppConstant(string name) => _config[$"AppConstants:{name}"];
    }
}
