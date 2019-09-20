using System.Threading.Tasks;
using Adsboard.Common.Types;

namespace Adsboard.Common.Dispatchers
{
    public interface IQueryDispatcher
    {
        Task<TResult> QueryAsync<TResult>(IQuery<TResult> query);
    }
}
