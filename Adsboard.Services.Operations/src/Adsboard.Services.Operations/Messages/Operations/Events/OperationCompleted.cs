using System;
using Adsboard.Common.Messages;
using Newtonsoft.Json;

namespace Adsboard.Services.Operations.Messages.Operations.Events
{
    public class OperationCompleted : IEvent
    {
        public Guid Id { get; }
        public Guid UserId { get; }
        public string Name { get; }
        public string Resource { get; }

        [JsonConstructor]
        public OperationCompleted(Guid id,
            Guid userId, string name, string resource)
        {
            Id = id;
            UserId = userId;
            Name = name;
            Resource = resource;
        }
    }
}