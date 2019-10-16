using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Adsboard.Api.Controllers
{
    [Route("")]
    public class HomeController : ControllerBase
    {
        public HomeController()
        {
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult Get() => Ok("Adsboard API");
    }
}