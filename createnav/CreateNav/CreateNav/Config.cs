using Microsoft.Extensions.Configuration;

using System;
using System.IO;

namespace CreateNav
{
    public class Config
    {
        private IConfigurationRoot _configuration;

        public Config()
        {
            _configuration = new ConfigurationBuilder()
                   .SetBasePath(Directory.GetCurrentDirectory())
                   .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                   .Build();
        }

        private string GetConfigValue(string value)
        {
            return _configuration.GetSection(value).Value;
        }

        public string RepoPath
        {
            get { return GetConfigValue("LocalRepo"); }
        }

        public string DocumentRoot
        {
            get { return GetConfigValue("DocumentRoot"); }
        }

        public Version Currentversion
        {
            get { return new Version(GetConfigValue("Version")); }
        }

        public string BlogName
        {
            get { return GetConfigValue("BlogName"); }
        }
    }
}