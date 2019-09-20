using Microsoft.AspNetCore.Mvc;

namespace Adsboard.Identity.Controllers
{
    [Route("")]
    public class HomeController : ControllerBase
    {
        [HttpGet]
        public IActionResult Get() => Ok("Identity Service");
    }
}