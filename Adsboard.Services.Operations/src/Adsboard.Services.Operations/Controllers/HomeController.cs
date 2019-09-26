using Microsoft.AspNetCore.Mvc;

namespace Adsboard.Services.Operations.Controllers
{
    [Route("")]
    public class HomeController : ControllerBase
    {
        [HttpGet]
        public IActionResult Get() => Ok("Adsboard Operations Service");
    }
}