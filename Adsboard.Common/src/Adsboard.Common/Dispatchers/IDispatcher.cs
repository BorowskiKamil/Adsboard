using System.Threading.Tasks;
using Adsboard.Common.Messages;
using Adsboard.Common.Types;

namespace Adsboard.Common.Dispatchers
{
    public interface IDispatcher
    {
        Task SendAsync<TCommand>(TCommand command) where TCommand : ICommand;
        Task<TResult> QueryAsync<TResult>(IQuery<TResult> query);
    }
}
