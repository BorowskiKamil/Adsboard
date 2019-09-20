using System.Threading.Tasks;
using Adsboard.Common.Types;

namespace Adsboard.Common.Handlers
{
    public interface IQueryHandler<TQuery,TResult> where TQuery : IQuery<TResult>
    {
        Task<TResult> HandleAsync(TQuery query);
    }
}