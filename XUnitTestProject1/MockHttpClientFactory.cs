using System;
using System.Net.Http;

namespace XUnitTestProject1
{
    public class MockHttpClientFactory : IHttpClientFactory
    {
        public Func<HttpClient> InjectHttpClient { get; set; }

        public HttpClient CreateClient(string name)
        {
            return InjectHttpClient();
        }
    }
}