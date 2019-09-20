using System;
using Microsoft.AspNetCore.Mvc;

namespace Adsboard.Identity.Controllers
{
    public class BaseController : ControllerBase
    {
        protected Guid UserId
            => string.IsNullOrWhiteSpace(User?.Identity?.Name) ? 
                Guid.Empty : 
                Guid.Parse(User.Identity.Name);
    }
}
