using System;
using System.Linq;
using System.Reflection;
using Adsboard.Common.Messages;
using Chronicle;

namespace Adsboard.Services.Operations.Sagas
{
    internal static class Extensions
    {
        private static readonly Type[] SagaTypes = Assembly.GetExecutingAssembly()
            .GetTypes()
            .Where(t => typeof(ISaga).IsAssignableFrom(t))
            .ToArray();

        internal static bool BelongsToSaga<TMessage>(this TMessage _) where TMessage : IMessage
        {
            return SagaTypes.Any(t => typeof(ISagaAction<TMessage>).IsAssignableFrom(t));
        }
    }
}