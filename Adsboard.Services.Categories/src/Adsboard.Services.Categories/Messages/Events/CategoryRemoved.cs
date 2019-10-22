using System;
using Adsboard.Common.Messages;
using Newtonsoft.Json;

namespace Adsboard.Services.Categories.Messages.Events
{
    public class CategoryRemoved : IEvent
    {
        public Guid Id { get; }
        public Guid UserId { get; }

        [JsonConstructor]
        public CategoryRemoved(Guid id, Guid userId)
        {
            Id = id;
            UserId = userId;
        }
    }
}