using System;
using System.Net.Http.Headers;
using Azure.Identity;
using Azure.Security.KeyVault.Secrets;
using Microsoft.Azure.Cosmos;
using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Poe.Functions.Config;
using Poe.Redis;
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
            
            var poeConfig = new PoEConfig();
            config.GetSection("PoEConfig").Bind(poeConfig);
            
            var redisConfig = new RedisConfig();
            config.GetSection("RedisConfig").Bind(redisConfig);
            builder.Services.AddSingleton(redisConfig);
            
            builder.Services.AddSingleton<RedisClient>(serviceProvider =>
            {
                var config = serviceProvider.GetRequiredService<RedisConfig>();
                return new RedisClient(config);
            });



            
            var cosmosConfig = new CosmosConfig();
            config.GetSection("CosmosConfig").Bind(cosmosConfig);
            builder.Services.AddSingleton(cosmosConfig);
            
            builder.Services.AddTransient<CosmosClient>(s =>
            {
                var configuration = s.GetService<IConfiguration>();
                return new CosmosClient(
                    cosmosConfig.ConnectionString);
            });

            
            builder.Services.AddTransient<ICosmosService, CosmosService>();


            // Registering services
            builder.Services.AddTransient<IGetProfileInfo, GetProfileInfo>();
            builder.Services.AddTransient<IGetStashService, GetStashService>();
            builder.Services.AddTransient<IGetCurrencyItems, GetCurrencyItems>();
            builder.Services.AddTransient<IGetEssenceItems, GetEssenceItems>();

            // HTTP client setup
            builder.Services.AddHttpClient<IGetStashService, GetStashService>(client =>
            {
                client.BaseAddress = new Uri(poeConfig.BaseApiAddress);
                client.DefaultRequestHeaders.Add("Accept", "application/json");
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", poeConfig.ApiToken);
            });
            
            builder.Services.AddHttpClient<IGetCurrencyItems, GetCurrencyItems>(client =>
            {
                client.BaseAddress = new Uri(poeConfig.BaseApiAddress);
                client.DefaultRequestHeaders.Add("Accept", "application/json");
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", poeConfig.ApiToken);
            });
            
            builder.Services.AddHttpClient<IGetEssenceItems, GetEssenceItems>(client =>
            {
                client.BaseAddress = new Uri(poeConfig.BaseApiAddress);
                client.DefaultRequestHeaders.Add("Accept", "application/json");
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", poeConfig.ApiToken);
            });
            
        }
    }
}