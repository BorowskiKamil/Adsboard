using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Adsboard.Common.Mvc;
using Adsboard.Common.Dispatchers;
using Adsboard.Services.Adverts.Messages.Commands;
using System.Collections.Generic;
using Adsboard.Services.Adverts.Dto;
using Adsboard.Services.Adverts.Queries;

namespace Adsboard.Services.Adverts.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class AdvertsController : ControllerBase
    {
        private readonly IDispatcher _dispatcher;

        public AdvertsController(IDispatcher dispatcher)
        {
            _dispatcher = dispatcher;
        }

        // [HttpGet]
        // public async Task<ActionResult<IEnumerable<AdvertDto>>> GetAds()
        //     => Ok(await _dispatcher.QueryAsync(new FindAdverts()));

        [HttpGet]
        public async Task<ActionResult<IEnumerable<AdvertDto>>> GetAdverts([FromQuery] FindAdverts query)
            => Ok(await _dispatcher.QueryAsync(query));

        [HttpGet("{id:guid}")]
        public async Task<ActionResult<AdvertDto>> Get([FromRoute] GetAdvert query)
        {
            var result = await _dispatcher.QueryAsync(query);
            if (result is null)
            {
                return NotFound();
            }

            return result;
        }

        [HttpPost]
        public async Task<ActionResult> Post(CreateAdvert command)
        {
            await _dispatcher.SendAsync(command.BindId(c => c.Id));
            return Accepted();
        }
    }
}