using Adsboard.Common.Messages;
using Adsboard.Common.RabbitMq;
using Adsboard.Services.Operations.Messages.Operations.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Adsboard.Services.Operations.Infrastructure
{
    public static class Subscriptions
    {
        private static readonly Assembly MessagesAssembly = typeof(Startup).Assembly;

        private static readonly ISet<Type> ExcludedMessages = new HashSet<Type>(new[]
        {
            typeof(OperationPending),
            typeof(OperationCompleted),
            typeof(OperationRejected)
        });

        public static IBusSubscriber SubscribeAllMessages(this IBusSubscriber subscriber)
            => subscriber.SubscribeAllCommands().SubscribeAllEvents();

        private static IBusSubscriber SubscribeAllCommands(this IBusSubscriber subscriber)
            => subscriber.SubscribeAllMessages<ICommand>(nameof(IBusSubscriber.SubscribeCommand));

        private static IBusSubscriber SubscribeAllEvents(this IBusSubscriber subscriber)
            => subscriber.SubscribeAllMessages<IEvent>(nameof(IBusSubscriber.SubscribeEvent));

        private static IBusSubscriber SubscribeAllMessages<TMessage>(this IBusSubscriber subscriber, string subscribeMethod)
        {
            var messageTypes = MessagesAssembly
                .GetTypes()
                .Where(t => t.IsClass && typeof(TMessage).IsAssignableFrom(t))
                .Where(t => !ExcludedMessages.Contains(t))
                .ToList();

            messageTypes.ForEach(mt => subscriber.GetType()
                .GetMethod(subscribeMethod)
                .MakeGenericMethod(mt)
                .Invoke(subscriber,
                    new object[] {mt.GetCustomAttribute<MessageNamespaceAttribute>()?.Namespace, null, null}));

            return subscriber;
        }
    }
}