using System;
using Adsboard.Common.Messages;
using Newtonsoft.Json;

namespace Adsboard.Services.Adverts.Messages.Events
{
    public class AdvertUpdated : IEvent
    {
        public Guid Id { get; }
        public Guid UserId { get; }

        [JsonConstructor]
        public AdvertUpdated(Guid id, Guid userId)
        {
            Id = id;
            UserId = userId;
        }
    }
}