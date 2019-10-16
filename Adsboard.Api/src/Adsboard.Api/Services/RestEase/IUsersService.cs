using System;
using System.Threading.Tasks;
using RestEase;

namespace Adsboard.Api.Services.RestEase
{
    [SerializationMethods(Query = QuerySerializationMethod.Serialized)]
    public interface IUsersService
    {
        [AllowAnyStatusCode]
        [Get("users/{id}")]
        Task<object> GetAsync([Path] Guid id);
    }
}