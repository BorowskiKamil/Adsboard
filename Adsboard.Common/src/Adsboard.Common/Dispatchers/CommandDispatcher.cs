using System;
using System.Threading.Tasks;
using Adsboard.Common.Handlers;
using Adsboard.Common.Messages;
using Adsboard.Common.RabbitMq;

namespace Adsboard.Common.Dispatchers
{
    public class CommandDispatcher : ICommandDispatcher
    {
        private readonly IServiceProvider _context;

        public CommandDispatcher(IServiceProvider context)
        {
            _context = context;
        }

        public async Task SendAsync<T>(T command) where T : ICommand
        {
            dynamic handler = _context.GetService(typeof(ICommandHandler<T>));

            await handler.HandleAsync((dynamic)command, CorrelationContext.Empty);
        }
    }
}