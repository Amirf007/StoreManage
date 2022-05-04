using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace StoreManage.Specs.Infrastructure
{
    public class ConfigurationFixture
    {
        public TestSettings Value { get; private set; }

        public ConfigurationFixture()
        {
            Value = GetSettings();
        }

        private TestSettings GetSettings()
        {
            var settings = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appSettings.json", optional: true, reloadOnChange: false)
                .AddEnvironmentVariables()
                .AddCommandLine(Environment.GetCommandLineArgs())
                .Build();

            var testSettings = new TestSettings();
            settings.Bind(testSettings);
            return testSettings;
        }
    }
    public class TestSettings
    {
        public string DbConnectionString { get; set; }
    }

    [CollectionDefinition(nameof(ConfigurationFixture), DisableParallelization = false)]
    public class ConfigurationCollectionFixture : ICollectionFixture<ConfigurationFixture>
    {
    }
}
