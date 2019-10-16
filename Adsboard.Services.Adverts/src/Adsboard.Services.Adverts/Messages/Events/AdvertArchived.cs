using System;
using Adsboard.Common.Messages;
using Newtonsoft.Json;

namespace Adsboard.Services.Adverts.Messages.Events
{
    public class AdvertArchived : IEvent
    {
        public Guid Id { get; }
        public Guid UserId { get; }
        public DateTimeOffset ArchivedAt { get; }

        [JsonConstructor]
        public AdvertArchived(Guid id, Guid userId, DateTimeOffset archivedAt)
        {
            Id = id;
            UserId = userId;
            ArchivedAt = archivedAt;
        }
    }
}