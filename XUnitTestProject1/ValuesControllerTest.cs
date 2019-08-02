using System.Net;
using System.Net.Http;
using LearningAspNetCoreIntegrationTesting;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using Xunit;
using Xunit.Abstractions;

namespace XUnitTestProject1
{
    public class ValuesControllerTest : IClassFixture<GenericWebApplicationFactory>
    {
        public ValuesControllerTest(GenericWebApplicationFactory factory, ITestOutputHelper output)
        {
            this.factory = factory;
            this.output = output;
        }

        protected GenericWebApplicationFactory factory;
        protected ITestOutputHelper output;


        [Fact]
        public void GetRequest_GatewayInaccessible_ShouldReturn502()
        {
            var client = factory.WithWebHostBuilder(p => p.ConfigureServices(services =>
            {
                services.PostConfigure<AppSettings>(options => { options.Url = "https://aaaaaaaa"; });
            })).CreateClient();
            var response = client.SendAsync(new HttpRequestMessage(HttpMethod.Get, "api/values")).Result;
            Assert.Equal(HttpStatusCode.BadGateway, response.StatusCode);
        }

        [Fact]
        public void GetRequest_GatewayOnFailed_ShouldReturn400()
        {
            var behavior = new Mock<IGatewayMockBehavior>();
            behavior.Setup(p => p.LogonResult()).Returns(new BadRequestResult());
            MockGatewayData.MockBehavior = behavior.Object;

            var client = CreateHttpClient();
            var response = client.SendAsync(new HttpRequestMessage(HttpMethod.Get, "api/values")).Result;
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }

        [Fact]
        public void GetRequest_GatewayOnSuccess_ShouldReturn200()
        {
            var behavior = new Mock<IGatewayMockBehavior>();
            behavior.Setup(p => p.LogonResult()).Returns(new ActionResult<string>("success"));
            MockGatewayData.MockBehavior = behavior.Object;

            var client = CreateHttpClient();
            var response = client.SendAsync(new HttpRequestMessage(HttpMethod.Get, "api/values")).Result;
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        private HttpClient CreateHttpClient()
        {
            var client = factory.WithWebHostBuilder(p => p.ConfigureServices(services =>
            {
                services.PostConfigure<AppSettings>(options => { options.Url = "http://localhost/gateway"; });

                services.AddSingleton(typeof(IHttpClientFactory), new MockHttpClientFactory
                {
                    InjectHttpClient = factory.CreateClient
                });
            })).CreateClient();

            return client;
        }
    }
}