using System;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;
using Arineta.Aws.Dto;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;

namespace Arineta.Aws.Proxy.Service.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsersManagementProxyController : ControllerBase
    {
        private readonly IConfiguration _configuration;

        public UsersManagementProxyController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [HttpPut]
        [Route("Add")]
        public async Task<IActionResult> Add([FromBody]UserDto userDto)
        {
            try
            {
                await SendHttpRequestAsync(nameof(Add), MethodType.Put, userDto);
                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex);
            }
        }

        [HttpGet]
        [Route("Get")]
        public async Task<IActionResult> Get()
        {
            try
            {
                var users = await SendHttpRequestAsync<object>(nameof(Get), MethodType.Get);
                return Ok(await users.Content.ReadAsStringAsync());
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex);
            }
        }

        [HttpPost]
        [Route("Delete")]
        public async Task<IActionResult> Delete(Guid id)
        {
            try
            {
                await SendHttpRequestAsync(nameof(Delete), MethodType.Delete, id);
                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex);
            }
        }

        private async Task<HttpResponseMessage> SendHttpRequestAsync<T>(string request, MethodType methodType, T record = default)
        {
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12 | SecurityProtocolType.Tls13;
            ServicePointManager.ServerCertificateValidationCallback = delegate { return true; };

            HttpResponseMessage response = default;

            using var client = new HttpClient(new HttpClientHandler
            {
                UseProxy = false,
                UseDefaultCredentials = false,
                ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => { return true; }
            });

            client.BaseAddress = new Uri(_configuration.GetSection("ServerAddress").Value);
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue(_configuration.GetSection("ContentType").Value));

            var x = new StringContent(JsonConvert.SerializeObject(record), Encoding.UTF8, _configuration.GetSection("ContentType").Value);

            var requestUri = Path.Combine(_configuration.GetSection("Api").Value, request);
            response = methodType switch
            {
                MethodType.Put => await client.PutAsJsonAsync(requestUri, record),
                MethodType.Get => await client.GetAsync(requestUri),
                MethodType.Delete => await client.PostAsJsonAsync(requestUri, record),
                _ => throw new ArgumentOutOfRangeException(nameof(methodType), methodType, null)
            };

            response.EnsureSuccessStatusCode();

            return response;
        }

        public enum MethodType
        {
            Put,
            Get,
            Delete
        }
    }
}
