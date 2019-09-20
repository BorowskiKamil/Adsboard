using System;
using Adsboard.Common.Messages;
using Adsboard.Common.Types;

namespace Adsboard.Common.RabbitMq
{
    public interface IBusSubscriber
    {
        IBusSubscriber SubscribeCommand<TCommand>(string @namespace = null, string queueName = null,
            Func<TCommand, AdsboardException, IRejectedEvent> onError = null)
            where TCommand : ICommand;

        IBusSubscriber SubscribeEvent<TEvent>(string @namespace = null, string queueName = null,
            Func<TEvent, AdsboardException, IRejectedEvent> onError = null) 
            where TEvent : IEvent;
    }
}
