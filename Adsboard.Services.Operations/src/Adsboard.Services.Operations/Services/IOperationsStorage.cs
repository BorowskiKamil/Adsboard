using Adsboard.Services.Operations.Dto;
using Adsboard.Services.Operations.Infrastructure;
using System;
using System.Threading.Tasks;

namespace Adsboard.Services.Operations.Services
{
    public interface IOperationsStorage
    {
        Task<OperationDto> GetAsync(Guid id);

        Task SetAsync(Guid id, Guid userId, string name,  OperationState state, 
            string resource, string code = null, string reason = null);
    }
}
