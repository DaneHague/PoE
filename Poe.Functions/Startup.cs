using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using PoE.Services;
using PoE.Services.Implementations;

[assembly: FunctionsStartup(typeof(Poe.Functions.Startup))]

namespace Poe.Functions
{
    public class Startup : FunctionsStartup
    {
        public override void Configure(IFunctionsHostBuilder builder)
        {
            builder.Services.AddSingleton<IGetProfileInfo, GetProfileInfo>();
        }
    }
}