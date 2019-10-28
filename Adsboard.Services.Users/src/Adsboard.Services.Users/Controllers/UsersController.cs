using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Adsboard.Common.Dispatchers;
using Adsboard.Services.Users.Dto;
using Adsboard.Services.Users.Queries;

namespace Adsboard.Services.Users.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IDispatcher _dispatcher;

        public UsersController(IDispatcher dispatcher)
        {
            _dispatcher = dispatcher;
        }

        [HttpGet("{id:guid}")]
        public async Task<ActionResult<UserDto>> Get([FromRoute] GetUser query)
        {
            var result = await _dispatcher.QueryAsync(query);
            if (result is null)
            {
                return NotFound();
            }

            return result;
        }
    }
}