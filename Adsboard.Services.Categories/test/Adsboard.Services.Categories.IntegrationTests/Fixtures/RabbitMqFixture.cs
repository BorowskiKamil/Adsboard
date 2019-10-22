using System;
using RawRabbit.Configuration;
using RawRabbit.Instantiation;
using RawRabbit;
using RawRabbit.Common;
using RawRabbit.Enrichers.MessageContext;
using BusClient = RawRabbit.Instantiation.Disposable.BusClient;
using System.Collections.Generic;
using Adsboard.Services.Categories.IntegrationTests.Config;
using Adsboard.Common.RabbitMq;
using Adsboard.Common;
using System.Threading.Tasks;
using Adsboard.Services.Categories.Domain;
using Adsboard.Common.Messages;
using Adsboard.Common.Domain;

namespace Adsboard.Services.Categories.IntegrationTests.Fixtures
{
    public class RabbitMqFixture : IDisposable
    {
        private readonly BusClient _client;
        private bool _disposed = false;

        public RabbitMqFixture()
        {
            _client = RawRabbitFactory.CreateSingleton(new RawRabbitOptions()
            {
                ClientConfiguration = new RawRabbitConfiguration
                {
                    Hostnames = new List<string> { "localhost" },
                    VirtualHost = "/",
                    Port = 5672,
                    Username = "guest",
                    Password = "guest",
                },
                DependencyInjection = ioc =>
                {
                    ioc.AddSingleton<INamingConventions>(new RabbitMqNamingConventions("categories"));
                },
                Plugins = p => p  
                    .UseAttributeRouting()
                    .UseRetryLater()
                    .UseMessageContext<CorrelationContext>()
                    .UseContextForwarding()
            });
        }

        public Task PublishAsync<TMessage>(TMessage message, string @namespace = null) where TMessage : class
            => _client.PublishAsync(message, ctx => 
                ctx.UseMessageContext(CorrelationContext.Empty).UsePublishConfiguration(p => p.WithRoutingKey(GetRoutingKey(@message, @namespace))));
        
        public async Task<TaskCompletionSource<TEntity>> SubscribeAndGetAsync<TEvent, TEntity>(
            Func<Guid, TaskCompletionSource<TEntity>, Task> onMessageReceived, Guid id) 
                where TEvent : IEvent
                where TEntity : Entity
        {
            var taskCompletionSource = new TaskCompletionSource<TEntity>();
            var guid = Guid.NewGuid().ToString();
            
            await _client.SubscribeAsync<TEvent>(
                async _ => await onMessageReceived(id, taskCompletionSource),
                ctx => ctx.UseSubscribeConfiguration(cfg =>
                    cfg
                        .FromDeclaredQueue(
                            builder => builder
                                .WithDurability(false)
                                .WithName(guid))));
            return taskCompletionSource;
        }
        
        private string GetRoutingKey<T>(T message, string @namespace = null)
        {
            @namespace = @namespace ?? "categories";
            @namespace = string.IsNullOrWhiteSpace(@namespace) ? string.Empty : $"{@namespace}.";

            return $"{@namespace}{typeof(T).Name.Underscore()}".ToLowerInvariant();
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    _client.Dispose();
                }
                _disposed = true;
            }
        }
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}