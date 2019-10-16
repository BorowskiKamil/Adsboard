using System;
using System.Linq;
using System.Threading.Tasks;
using Adsboard.Api.Authentication;
using Adsboard.Common.Messages;
using Adsboard.Common.RabbitMq;
using Microsoft.AspNetCore.Mvc;

namespace Adsboard.Api.Controllers
{
    [Route("[controller]")]
    [ApiController]
    [JwtAuth]
    public abstract class BaseController : ControllerBase
    {
        private static readonly string AcceptLanguageHeader = "accept-language";
        private static readonly string OperationHeader = "X-Operation";
        private static readonly string ResourceHeader = "X-Resource";
        private static readonly string DefaultCulture = "en-us";
        private static readonly string PageLink = "page";
        protected readonly IBusPublisher _busPublisher;

        public BaseController(IBusPublisher busPublisher)
        {
            _busPublisher = busPublisher;
        }

        protected ActionResult<T> Single<T>(T model, Func<T,bool> criteria = null)
        {
            if (model == null)
            {
                return NotFound();
            }
            var isValid = criteria == null || criteria(model);
            if (isValid)
            {
                return Ok(model);
            }

            return NotFound();
        }

        protected ActionResult<T> Result<T>(T model, Func<T, bool> criteria = null)
        {
            if (model == null)
            {
                return NotFound();
            }

            var isValid = criteria == null || criteria(model);
            if (isValid)
            {
                return Ok(model);
            }

            return NotFound();
        }

        protected async Task<IActionResult> SendAsync<T>(T command, 
            Guid? resourceId = null, string resource = "") where T : ICommand 
        {
            var context = GetContext<T>(resourceId, resource);
            await _busPublisher.SendAsync(command, context);

            return Accepted(context);
        }

        protected IActionResult Accepted(ICorrelationContext context)
        {
            Response.Headers.Add(OperationHeader, $"operations/{context.Id}");
            if (!string.IsNullOrWhiteSpace(context.Resource))
            {
                Response.Headers.Add(ResourceHeader, context.Resource);
            }

            return base.Accepted();
        }

        protected ICorrelationContext GetContext<T>(Guid? resourceId = null, string resource = "") where T : ICommand
        {
            if (!string.IsNullOrWhiteSpace(resource) && resourceId.HasValue)
            {
                resource = $"{resource}/{resourceId.Value.ToString()}";
            }

            return CorrelationContext.Create<T>(Guid.NewGuid(), UserId, resourceId ?? Guid.Empty, 
               HttpContext.TraceIdentifier, HttpContext.Connection.Id, "",
               Request.Path.ToString(), Culture, resource);
        }

        protected bool IsAdmin
            => User.IsInRole("admin");

        protected Guid UserId
            => string.IsNullOrWhiteSpace(User?.Identity?.Name) ? 
                Guid.Empty : 
                Guid.Parse(User.Identity.Name);

        protected string Culture 
            => Request.Headers.ContainsKey(AcceptLanguageHeader) ? 
                    Request.Headers[AcceptLanguageHeader].First().ToLowerInvariant() :
                    DefaultCulture;

        private string GetPageLink(int currentPage, int page)
        {
            var path = Request.Path.HasValue ? Request.Path.ToString() : string.Empty;
            var queryString = Request.QueryString.HasValue ?  Request.QueryString.ToString() : string.Empty;
            var conjunction = string.IsNullOrWhiteSpace(queryString) ? "?" : "&";
            var fullPath = $"{path}{queryString}";
            var pageArg = $"{PageLink}={page}";
            var link = fullPath.Contains($"{PageLink}=")
                ? fullPath.Replace($"{PageLink}={currentPage}", pageArg)
                : fullPath += $"{conjunction}{pageArg}";

            return link;
        }

        private static string FormatLink(string path, string rel)
            => string.IsNullOrWhiteSpace(path) ? string.Empty : $"<{path}>; rel=\"{rel}\",";
    }
}