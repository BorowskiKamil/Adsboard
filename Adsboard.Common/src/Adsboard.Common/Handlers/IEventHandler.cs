using System.Threading.Tasks;
using Adsboard.Common.Messages;
using Adsboard.Common.RabbitMq;

namespace Adsboard.Common.Handlers
{
    public interface IEventHandler<in TEvent> where TEvent : IEvent
    {
        Task HandleAsync(TEvent @event, ICorrelationContext context);
    }
}