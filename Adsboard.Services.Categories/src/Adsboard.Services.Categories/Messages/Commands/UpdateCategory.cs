using System;
using Adsboard.Common.Messages;
using Newtonsoft.Json;

namespace Adsboard.Services.Categories.Messages.Commands
{
    public class UpdateCategory : ICommand
    {
        public Guid Id { get; }
        public Guid UserId { get; }

        public string Name { get; }

        [JsonConstructor]
        public UpdateCategory(Guid id, string name)
        {
            Id = id;
            Name = name;
        }
    }
}