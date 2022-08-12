using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Azure.Extensions.AspNetCore.Configuration.Secrets;
using Azure.Identity;
using Azure.Security.KeyVault.Secrets;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;

namespace Application.Configuration
{
    public class ConfigCredentials
    {
        public static void ConfigureKeyVault(HostBuilderContext context, IConfigurationBuilder config)
        {
            var builtConfig = config.Build();
            var secretClient = new SecretClient(
            new Uri($"https://{builtConfig["KeyVaultName"]}.vault.azure.net/"),
            new DefaultAzureCredential());
            config.AddAzureKeyVault(secretClient, new KeyVaultSecretManager());
        }
        public string GoogleKey { get; set; }
        public string DropletUser { get; set; }
        public string DropletPassword { get; set; }
        public string DropletAddress { get; set; }
        public string AzureUsername { get; set; }
        public string AzurePassword { get; set; }

        public ConfigCredentials()
        {

        }
        public ConfigCredentials(IConfiguration config)
        {
            this.GoogleKey = config["googleKey"];
            this.DropletUser = config["DropletUser"];
            this.DropletPassword = config["DropletPassword"];
            this.DropletAddress = config["DropletAddress"];
            this.AzureUsername = config["AzureUsername"];
            this.AzurePassword = config["AzurePassword"];
        }
    }
}