using Microsoft.AspNetCore.Mvc;
using Adsboard.Common.Dispatchers;

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
    }
}