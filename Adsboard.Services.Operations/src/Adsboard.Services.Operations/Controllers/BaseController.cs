using System.Threading.Tasks;
using Adsboard.Common.Dispatchers;
using Adsboard.Common.Types;
using Microsoft.AspNetCore.Mvc;

namespace Adsboard.Services.Operations.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class BaseController : ControllerBase
    {
        private readonly IDispatcher _dispatcher;

        public BaseController(IDispatcher dispatcher)
        {
            _dispatcher = dispatcher;
        }

        protected async Task<TResult> QueryAsync<TQuery, TResult>(TQuery query) where TQuery : IQuery<TResult>
            => await _dispatcher.QueryAsync(query);

        protected ActionResult<T> Single<T>(T data)
        {
            if (data == null)
            {
                return NotFound();
            }

            return Ok(data);
        }
    }
}