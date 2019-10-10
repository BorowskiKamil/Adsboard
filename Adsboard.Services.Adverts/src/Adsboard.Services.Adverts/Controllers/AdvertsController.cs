using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Adsboard.Common.Mvc;
using Adsboard.Common.Dispatchers;
using Adsboard.Services.Adverts.Messages.Commands;

namespace Bookmarkly.Cloud.Bookmarks.Controllers
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

        [HttpPost]
        public async Task<ActionResult> Post(CreateAdvert command)
        {
            await _dispatcher.SendAsync(command.BindId(c => c.Id));
            return Accepted();
        }
    }
}