using System;
using Adsboard.Common.Messages;
using Newtonsoft.Json;

namespace Adsboard.Api.Messages.Categories
{
    [MessageNamespace("categories")]
    public class RemoveCategory : ICommand
    {
        public Guid Id { get; }
        public Guid UserId { get; }

        [JsonConstructor]
        public RemoveCategory(Guid id, Guid userId)
        {
            Id = id;
            UserId = userId;
        }
    }
}