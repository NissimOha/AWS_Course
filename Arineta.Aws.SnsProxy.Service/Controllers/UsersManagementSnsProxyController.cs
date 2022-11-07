using System;
using System.Text.Json;
using System.Threading.Tasks;
using Amazon.Runtime;
using Amazon.SimpleNotificationService;
using Amazon.SimpleNotificationService.Model;
using Arineta.Aws.Dto;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace Arineta.Aws.Proxy.Service.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsersManagementSnsProxyController : ControllerBase
    {
        private readonly IConfiguration _configuration;

        public UsersManagementSnsProxyController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [HttpPut]
        [Route("Add")]
        public async Task<IActionResult> Add([FromBody]UserDto userDto)
        {
            try
            {
                return await PublishAsync(userDto,
                    _configuration.GetSection("AddSubject").Value,
                    _configuration.GetSection("AddTopic").Value);
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
                return await PublishAsync(id,
                    _configuration.GetSection("DeleteSubject").Value,
                    _configuration.GetSection("DeleteTopic").Value);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex);
            }
        }

        private async Task<IActionResult> PublishAsync<T>(T data, string subject, string topic)
        {
            //Todo: get credentials from shared AWS credentials file or the SDK Store
            using var client = new AmazonSimpleNotificationServiceClient(
                //new BasicAWSCredentials(_configuration.GetSection("AccessKey").Value,
                //_configuration.GetSection("SecretKey").Value)
                //,
                new AmazonSimpleNotificationServiceConfig
                {
                    RegionEndpoint = Amazon.RegionEndpoint.USEast1
                });

            var request = new PublishRequest()
            {
                Subject = subject,
                Message = JsonSerializer.Serialize(data),
                TopicArn = topic,
            };

            var response = await client.PublishAsync(request);

            return Ok(response);
        }
    }
}
