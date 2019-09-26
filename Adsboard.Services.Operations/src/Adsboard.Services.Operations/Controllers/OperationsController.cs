using System;
using System.Threading.Tasks;
using Adsboard.Common.Dispatchers;
using Adsboard.Services.Operations.Dto;
using Adsboard.Services.Operations.Services;
using Microsoft.AspNetCore.Mvc;

namespace Adsboard.Services.Operations.Controllers
{
    [Route("[controller]")]
    public class OperationsController : BaseController
    {
        private readonly IOperationsStorage _operationsStorage;

        public OperationsController(IDispatcher dispatcher,
            IOperationsStorage operationsStorage) : base(dispatcher)
        {
            _operationsStorage = operationsStorage;
        }
        
        [HttpGet("{id}")]
        public async Task<ActionResult<OperationDto>> Get(Guid id)
            => Single(await _operationsStorage.GetAsync(id));
    }
}