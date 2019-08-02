using System.Net.Http;
using LearningAspNetCoreIntegrationTesting;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;

namespace XUnitTestProject1
{
    public class GenericWebApplicationFactory : WebApplicationFactory<Startup>
    {
        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.ConfigureServices(services =>
            {
                services.AddMvc().AddApplicationPart(typeof(MockGatewayController).Assembly).AddControllersAsServices();
            });
        }
    }
}