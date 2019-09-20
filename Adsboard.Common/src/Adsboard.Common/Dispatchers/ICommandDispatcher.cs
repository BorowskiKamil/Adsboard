using System.Threading.Tasks;
using Adsboard.Common.Messages;

namespace Adsboard.Common.Dispatchers
{
    public interface ICommandDispatcher
    {
         Task SendAsync<T>(T command) where T : ICommand;
    }
}