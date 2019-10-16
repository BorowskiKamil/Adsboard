using System;
using System.Threading.Tasks;
using Adsboard.Api.Models.Operations;
using RestEase;

namespace Adsboard.Api.Services.RestEase
{
    public interface IOperationsService
    {
        [AllowAnyStatusCode]
        [Get("operations/{id}")]
        Task<Operation> GetAsync([Path] Guid id);          

    }
}