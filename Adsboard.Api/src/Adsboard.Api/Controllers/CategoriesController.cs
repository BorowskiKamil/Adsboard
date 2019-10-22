using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Adsboard.Common.RabbitMq;
using Adsboard.Common.Mvc;
using Adsboard.Api.Services.RestEase;
using Adsboard.Api.Queries.Adverts;
using Adsboard.Api.Messages.Adverts;
using System;
using Adsboard.Services.Categories.Queries;
using Adsboard.Api.Messages.Categories;

namespace Adsboard.Api.Controllers
{
    public class CategoriesController : BaseController
    {
        private readonly ICategoriesService _categoriesService;

        public CategoriesController(IBusPublisher busPublisher, ICategoriesService categoriesService) 
            : base(busPublisher)
        {
            _categoriesService = categoriesService;
        }

        [HttpGet]
        public async Task<ActionResult<object>> GetAllCategories([FromQuery] FindCategories query)
            => await _categoriesService.FindAsync(query);

        [HttpGet("{id:guid}")]
        public async Task<ActionResult<object>> GetDetails(Guid id)
            => Single(await _categoriesService.GetAsync(id));

        [HttpPost]
        public async Task<IActionResult> Post(CreateCategory command)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(command);
            }

            return await SendAsync(command.BindId(c => c.Id).Bind(c => c.UserId, UserId), resourceId: command.Id,
                            resource: "categories");
        }

        [HttpPatch("{id:guid}")]
        public async Task<IActionResult> UpdateAdvert(Guid id, [FromBody] UpdateCategory command)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(command);
            }

            return await SendAsync(command.Bind(c => c.Id, id).Bind(c => c.UserId, UserId), resourceId: command.Id,
                            resource: "categories");
        }

        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var command = new ArchiveAdvert(id, UserId);
            return await SendAsync(command, resourceId: command.Id,
                            resource: "categories");
        }
    }
}