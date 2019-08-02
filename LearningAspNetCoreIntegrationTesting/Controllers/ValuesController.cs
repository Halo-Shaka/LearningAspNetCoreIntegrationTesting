using System;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;

namespace LearningAspNetCoreIntegrationTesting.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ValuesController : ControllerBase
    {
        private readonly IHttpClientFactory _httpClientFactory;

        private readonly IOptions<AppSettings> _options;

        public ValuesController(IHttpClientFactory httpClientFactory, IOptions<AppSettings> options)
        {
            _httpClientFactory = httpClientFactory;
            _options = options;
        }

        // GET api/values
        [HttpGet]
        public async Task<ActionResult> Get()
        {
            var client = _httpClientFactory.CreateClient();

            var url = _options.Value.Url;
            var payload = new
            {
                From = "China"
            };

            var requestMessage = new HttpRequestMessage(HttpMethod.Post, url)
            {
                Content = new StringContent(JsonConvert.SerializeObject(payload), Encoding.UTF8, "application/json")
            };

            try
            {
                var response = await client.SendAsync(requestMessage);
                var content = await response.Content.ReadAsStringAsync();

                if (response.StatusCode == HttpStatusCode.OK)
                {
                    return Ok(content);
                }

                return BadRequest();
            }
            catch (Exception e)
            {
                return StatusCode(502);
            }
        }
    }
}