using System;
using System.Threading.Tasks;
using Arineta.Aws.Common.Entities;
using Arineta.Aws.Common.IFC;
using Arineta.Aws.Dto;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Arineta.Aws.Service.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsersManagementController : ControllerBase
    {
        private readonly IManagement<User> _management;
        private readonly IMapper _mapper;

        public UsersManagementController(IManagement<User> management, IMapper mapper)
        {
            _management = management;
            _mapper = mapper;
        }

        [HttpPut]
        [Route("Add")]
        public async Task<IActionResult> Add([FromBody]UserDto userDto)
        {
            try
            {
                var user = _mapper.Map<User>(userDto);
                await _management.AddAsync(user);
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
                var users = await _management.GetAsync();
                return Ok(users);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex);
            }
        }

        [HttpPost]
        [Route("Delete")]
        public async Task<IActionResult> Delete([FromBody]Guid id)
        {
            try
            {
                await _management.DeleteAsync(id);
                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex);
            }
        }
    }
}
