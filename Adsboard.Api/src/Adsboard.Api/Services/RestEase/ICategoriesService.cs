using System;
using System.Threading.Tasks;
using Adsboard.Services.Categories.Queries;
using RestEase;

namespace Adsboard.Api.Services.RestEase
{
    [SerializationMethods(Query = QuerySerializationMethod.Serialized)]
    public interface ICategoriesService
    {
        [AllowAnyStatusCode]
        [Get("categories/{id}")]
        Task<object> GetAsync([Path] Guid id);  
        
        [AllowAnyStatusCode]
        [Get("categories")]
        Task<object> FindAsync(FindCategories query); 
    }
}