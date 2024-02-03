using System;
using Azure.Identity;
using Azure.Security.KeyVault.Secrets;
using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Poe.Functions.Config;
using PoE.Services;
using PoE.Services.Implementations;

[assembly: FunctionsStartup(typeof(Poe.Functions.Startup))]

namespace Poe.Functions
{
    public class Startup : FunctionsStartup
    {
        public override void Configure(IFunctionsHostBuilder builder)
        {
            var builtConfig = new ConfigurationBuilder()
                .AddEnvironmentVariables() // Load environment variables
                .Build();

            var credential = new ClientSecretCredential(
                builtConfig["AZURE_TENANT_ID"], 
                builtConfig["AZURE_CLIENT_ID"], 
                builtConfig["AZURE_CLIENT_SECRET"]);

            var config = new ConfigurationBuilder()
                .AddAzureKeyVault(new Uri(builtConfig["KEYVAULT_URL"]), credential)
                .Build();
    
            // Assuming you want to bind directly here
            var poeConfig = new PoEConfig();
            config.GetSection("PoEConfig").Bind(poeConfig);

            // Registering services
            builder.Services.AddTransient<IGetProfileInfo, GetProfileInfo>();
            builder.Services.AddTransient<IGetStashService, GetStashService>();

            // HTTP client setup
            builder.Services.AddHttpClient<IGetStashService, GetStashService>(client =>
            {
                client.BaseAddress = new Uri(poeConfig.BaseApiAddress);
                client.DefaultRequestHeaders.Add("Accept", "application/json");
            });
        }
    }
}