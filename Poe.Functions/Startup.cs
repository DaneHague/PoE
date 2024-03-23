using System;
using System.Net.Http.Headers;
using Azure.Identity;
using Microsoft.Azure.Cosmos;
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
            
            var poeConfig = new PoEConfig();
            config.GetSection("PoEConfig").Bind(poeConfig);
            
            var cosmosConfig = new CosmosConfig();
            config.GetSection("CosmosConfig").Bind(cosmosConfig);
            builder.Services.AddSingleton(cosmosConfig);
            
            builder.Services.AddTransient<CosmosClient>(s =>
            {
                var configuration = s.GetService<IConfiguration>();
                return new CosmosClient(
                    cosmosConfig.ConnectionString);
            });
            
            // Registering services
            builder.Services.AddTransient<IGetProfileInfo, GetProfileInfo>();
            builder.Services.AddTransient<IGetStashService, GetStashService>();
            builder.Services.AddTransient<IGetCurrencyItems, GetCurrencyItems>();
            builder.Services.AddTransient<IGetEssenceItems, GetEssenceItems>();
            builder.Services.AddTransient<IGetTradeRequestResponseService, GetTradeRequestResponseService>();
            builder.Services.AddTransient<ITimerTriggerService, TimerTriggerService>();
            
            builder.Services.AddTransient<ICosmosService, CosmosService>();

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
            
            builder.Services.AddHttpClient<IGetTradeRequestResponseService, GetTradeRequestResponseService>(client =>
            {
                client.BaseAddress = new Uri("https://www.pathofexile.com/api/trade/search/affliction");
                client.DefaultRequestHeaders.Add("Accept", "application/json");
                client.DefaultRequestHeaders.Add("User-Agent", "PostmanRuntime/7.32.2");
                //client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", poeConfig.ApiToken);
            });
            
        }
    }
}