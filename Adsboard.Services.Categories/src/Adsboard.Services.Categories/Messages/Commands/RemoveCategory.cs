using System;
using Adsboard.Common.Messages;
using Newtonsoft.Json;

namespace Adsboard.Services.Categories.Messages.Commands
{
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