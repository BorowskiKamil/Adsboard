using System;
using System.Threading.Tasks;
using Adsboard.Common.Handlers;
using Adsboard.Common.Messages;
using Adsboard.Common.Types;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Polly;
using RawRabbit;
using RawRabbit.Common;
using RawRabbit.Enrichers.MessageContext;

namespace Adsboard.Common.RabbitMq
{
    public class BusSubscriber : IBusSubscriber
    {
        private readonly ILogger _logger;
        private readonly IBusClient _busClient;
        private readonly IServiceProvider _serviceProvider;        
        private readonly int _retries;
        private readonly int _retryInterval;

        public BusSubscriber(IApplicationBuilder app)
        {
            _logger = app.ApplicationServices.GetService<ILogger<BusSubscriber>>();
            _serviceProvider = app.ApplicationServices.GetService<IServiceProvider>();
            _busClient = _serviceProvider.GetService<IBusClient>();
            var options = _serviceProvider.GetService<IOptions<RabbitMqOptions>>().Value;
            _retries = options.Retries >= 0 ? options.Retries : 3;
            _retryInterval = options.RetryInterval > 0 ? options.RetryInterval : 2;
        }

        public IBusSubscriber SubscribeCommand<TCommand>(string @namespace = null, string queueName = null,
            Func<TCommand, AdsboardException, IRejectedEvent> onError = null)
            where TCommand : ICommand
        {
            _busClient.SubscribeAsync<TCommand, CorrelationContext>(async (command, correlationContext) =>
            {
                using (var scope = _serviceProvider.CreateScope()) 
                {
                    var commandHandler = scope.ServiceProvider.GetService<ICommandHandler<TCommand>>();
                    return await TryHandleAsync(command, correlationContext,
                        () => commandHandler.HandleAsync(command, correlationContext), onError);                       
                }
            });

            return this;
        }

        public IBusSubscriber SubscribeEvent<TEvent>(string @namespace = null, string queueName = null,
            Func<TEvent, AdsboardException, IRejectedEvent> onError = null)
            where TEvent : IEvent
        {
            _busClient.SubscribeAsync<TEvent, CorrelationContext>(async (@event, correlationContext) =>
            {
                using (var scope = _serviceProvider.CreateScope())
                {
                    var eventHandler = scope.ServiceProvider.GetService<IEventHandler<TEvent>>();

                    return await TryHandleAsync(@event, correlationContext,
                        () => eventHandler.HandleAsync(@event, correlationContext), onError);
                }
            });

            return this;
        }

        private async Task<Acknowledgement> TryHandleAsync<TMessage>(TMessage message,
            CorrelationContext correlationContext,
            Func<Task> handle, Func<TMessage, AdsboardException, IRejectedEvent> onError = null)
        {
            var currentRetry = 0;
            var retryPolicy = Policy
                .Handle<Exception>()
                .WaitAndRetryAsync(_retries, i => TimeSpan.FromSeconds(_retryInterval));
            
            var messageName = message.GetType().Name;

            return await retryPolicy.ExecuteAsync<Acknowledgement>(async () =>
            {                    
                try
                {
                    var retryMessage = currentRetry == 0
                        ? string.Empty
                        : $"Retry: {currentRetry}'.";

                    var preLogMessage = $"Handling a message: '{messageName}' " +
                                    $"with correlation id: '{correlationContext.Id}'. {retryMessage}";
                    
                    _logger.LogInformation(preLogMessage);

                    await handle();

                    var postLogMessage = $"Handled a message: '{messageName}' " +
                                            $"with correlation id: '{correlationContext.Id}'. {retryMessage}";
                    _logger.LogInformation(postLogMessage);

                    return new Ack();
                }
                catch (Exception exception)
                {
                    currentRetry++;
                    _logger.LogError(exception, exception.Message);
                    
                    if (exception is AdsboardException adsboardException && onError != null)
                    {
                        var rejectedEvent = onError(message, adsboardException);
                        await _busClient.PublishAsync(rejectedEvent, ctx => ctx.UseMessageContext(correlationContext));
                        _logger.LogInformation($"Published a rejected event: '{rejectedEvent.GetType().Name}' " +
                                                $"for the message: '{messageName}' with correlation id: '{correlationContext.Id}'.");

                        // span.SetTag("error-type", "domain");
                        return new Ack();
                    }

                    // span.SetTag("error-type", "infrastructure");
                    throw new Exception($"Unable to handle a message: '{messageName}' " +
                                        $"with correlation id: '{correlationContext.Id}', " +
                                        $"retry {currentRetry - 1}/{_retries}...");
                }
            // }
            });
        }
        
        // RabbitMQ retry that will publish a message to the retry queue.
        // Keep in mind that it might get processed by the other services using the same routing key and wildcards.
        private async Task<Acknowledgement> TryHandleWithRequeuingAsync<TMessage>(TMessage message,
            CorrelationContext correlationContext,
            Func<Task> handle, Func<TMessage, AdsboardException, IRejectedEvent> onError = null)
        {
            var messageName = message.GetType().Name;
            var retryMessage = correlationContext.Retries == 0
                ? string.Empty
                : $"Retry: {correlationContext.Retries}'.";
            _logger.LogInformation($"Handling a message: '{messageName}' " +
                                   $"with correlation id: '{correlationContext.Id}'. {retryMessage}");

            try
            {
                await handle();
                _logger.LogInformation($"Handled a message: '{messageName}' " +
                                       $"with correlation id: '{correlationContext.Id}'. {retryMessage}");

                return new Ack();
            }
            catch (Exception exception)
            {
                _logger.LogError(exception, exception.Message);
                if (exception is AdsboardException adsboardException && onError != null)
                {
                    var rejectedEvent = onError(message, adsboardException);
                    await _busClient.PublishAsync(rejectedEvent, ctx => ctx.UseMessageContext(correlationContext));
                    _logger.LogInformation($"Published a rejected event: '{rejectedEvent.GetType().Name}' " +
                                           $"for the message: '{messageName}' with correlation id: '{correlationContext.Id}'.");

                    return new Ack();
                }

                if (correlationContext.Retries >= _retries)
                {
                    await _busClient.PublishAsync(RejectedEvent.For(messageName),
                        ctx => ctx.UseMessageContext(correlationContext));

                    throw new Exception($"Unable to handle a message: '{messageName}' " +
                                        $"with correlation id: '{correlationContext.Id}' " +
                                        $"after {correlationContext.Retries} retries.", exception);
                }

                _logger.LogInformation($"Unable to handle a message: '{messageName}' " +
                                       $"with correlation id: '{correlationContext.Id}', " +
                                       $"retry {correlationContext.Retries}/{_retries}...");

                return Retry.In(TimeSpan.FromSeconds(_retryInterval));
            }
        }
    }
}