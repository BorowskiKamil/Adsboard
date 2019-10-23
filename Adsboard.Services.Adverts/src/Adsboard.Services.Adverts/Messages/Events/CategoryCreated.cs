using System;
using Adsboard.Common.Messages;
using Newtonsoft.Json;

namespace Adsboard.Services.Adverts.Messages.Events
{
    [MessageNamespace("categories")]
    public class CategoryCreated : IEvent
    {
        public Guid Id { get; }
        public Guid UserId { get; }
        public string Name { get; }

        [JsonConstructor]
        public CategoryCreated(Guid id, string name, Guid userId)
        {
            Id = id;
            Name = name;
            UserId = userId;
        }
    }
}