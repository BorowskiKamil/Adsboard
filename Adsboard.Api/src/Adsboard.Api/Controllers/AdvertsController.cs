using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Adsboard.Common.RabbitMq;
using Adsboard.Common.Mvc;
using Adsboard.Api.Services.RestEase;
using Adsboard.Api.Queries.Adverts;
using Adsboard.Api.Messages.Adverts;
using System;

namespace Adsboard.Api.Controllers
{
    public class AdvertsController : BaseController
    {
        private readonly IAdvertsService _advertsService;

        public AdvertsController(IBusPublisher busPublisher, IAdvertsService advertsService) 
            : base(busPublisher)
        {
            _advertsService = advertsService;
        }

        [HttpGet("me")]
        public async Task<ActionResult<object>> GetLoggedUserAdverts([FromQuery] FindAdverts query)
            => await _advertsService.FindAsync(query.Bind(x => x.UserId, UserId));

        [HttpGet]
        public async Task<ActionResult<object>> GetAllAdverts([FromQuery] FindAdverts query)
            => await _advertsService.FindAsync(query);

        [HttpGet("{id:guid}")]
        public async Task<ActionResult<object>> GetDetails(Guid id)
            => Single(await _advertsService.GetAsync(id));

        [HttpPost]
        public async Task<IActionResult> Post(CreateAdvert command)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(command);
            }

            return await SendAsync(command.BindId(c => c.Id).Bind(c => c.UserId, UserId), resourceId: command.Id,
                            resource: "adverts");
        }

        [HttpPatch("{id:guid}")]
        public async Task<IActionResult> UpdateAdvert(Guid id, [FromBody]UpdateAdvert command)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(command);
            }

            return await SendAsync(command.Bind(c => c.Id, id).Bind(c => c.UserId, UserId), resourceId: command.Id,
                            resource: "adverts");
        }

        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> Archive(Guid id)
        {
            var command = new ArchiveAdvert(id, UserId);
            return await SendAsync(command, resourceId: command.Id,
                            resource: "adverts");
        }
    }
}