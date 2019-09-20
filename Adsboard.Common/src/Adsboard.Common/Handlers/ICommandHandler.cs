using System.Threading.Tasks;
using Adsboard.Common.Messages;
using Adsboard.Common.RabbitMq;

namespace Adsboard.Common.Handlers
{
    public interface ICommandHandler<in TCommand> where TCommand : ICommand
    {
        Task HandleAsync(TCommand command, ICorrelationContext context);
    }
}