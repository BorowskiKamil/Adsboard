using System;
using System.Threading.Tasks;
using Adsboard.Api.Queries.Adverts;
using RestEase;

namespace Adsboard.Api.Services.RestEase
{
    [SerializationMethods(Query = QuerySerializationMethod.Serialized)]
    public interface IAdvertsService
    {
        [AllowAnyStatusCode]
        [Get("adverts/{id}")]
        Task<object> GetAsync([Path] Guid id);  
        
        [AllowAnyStatusCode]
        [Get("adverts")]
        Task<object> FindAsync(FindAdverts query); 
    }
}