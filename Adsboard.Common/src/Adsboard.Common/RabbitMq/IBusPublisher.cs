using System.Threading.Tasks;
using Adsboard.Common.Messages;

namespace Adsboard.Common.RabbitMq
{
    public interface IBusPublisher
    {
        Task SendAsync<TCommand>(TCommand command, ICorrelationContext context)
            where TCommand : ICommand;

        Task PublishAsync<TEvent>(TEvent @event, ICorrelationContext context)
            where TEvent : IEvent;
    }
}