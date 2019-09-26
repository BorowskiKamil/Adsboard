using System;
using System.Threading.Tasks;
using Adsboard.Common.Handlers;
using Adsboard.Common.Messages;
using Adsboard.Common.RabbitMq;
using Adsboard.Services.Operations.Sagas;
using Adsboard.Services.Operations.Services;
using Chronicle;
using SagaContext = Adsboard.Services.Operations.Sagas.SagaContext;

namespace Adsboard.Services.Operations.Handlers
{
    public class GenericCommandHandler<T> : ICommandHandler<T> where T : class, ICommand
    {
        private readonly ISagaCoordinator _sagaCoordinator;
        private readonly IOperationPublisher _operationPublisher;
        private readonly IOperationsStorage _operationsStorage;

        public GenericCommandHandler(ISagaCoordinator sagaCoordinator,
            IOperationPublisher operationPublisher,
            IOperationsStorage operationsStorage)
        {
            _sagaCoordinator = sagaCoordinator;
            _operationPublisher = operationPublisher;
            _operationsStorage = operationsStorage;
        }

        public async Task HandleAsync(T command, ICorrelationContext context)
        {
            if (!command.BelongsToSaga())
            {
                return;
            }

            var sagaContext = SagaContext.FromCorrelationContext(context);
            await _sagaCoordinator.ProcessAsync(command, sagaContext);
        }
    }
}