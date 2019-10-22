using Microsoft.AspNetCore.Mvc;
using Adsboard.Common.Dispatchers;
using System.Threading.Tasks;
using Adsboard.Services.Categories.Messages.Commands;
using Adsboard.Common.Mvc;
using Adsboard.Services.Categories.Dto;
using System.Collections.Generic;
using Adsboard.Services.Categories.Queries;

namespace Adsboard.Services.Categories.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class CategoriesController : ControllerBase
    {
        private readonly IDispatcher _dispatcher;

        public CategoriesController(IDispatcher dispatcher)
        {
            _dispatcher = dispatcher;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<CategoryDto>>> GetCategories ([FromQuery] FindCategories query)
            => Ok(await _dispatcher.QueryAsync(query));

        [HttpGet("{id:guid}")]
        public async Task<ActionResult<CategoryDto>> Get([FromRoute] GetCategory query)
        {
            var result = await _dispatcher.QueryAsync(query);
            if (result is null)
            {
                return NotFound();
            }

            return result;
        }

        [HttpPost]
        public async Task<ActionResult> Post(CreateCategory command)
        {
            await _dispatcher.SendAsync(command.BindId(c => c.Id));
            return Accepted();
        }
    }
}