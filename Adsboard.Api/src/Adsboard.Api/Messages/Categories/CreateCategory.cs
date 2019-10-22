using System;
using Adsboard.Common.Messages;
using Newtonsoft.Json;

namespace Adsboard.Api.Messages.Categories
{
    [MessageNamespace("categories")]
    public class CreateCategory : ICommand
    {
        public Guid Id { get; }
        public string Name { get; }
        public Guid UserId { get; }

        [JsonConstructor]
        public CreateCategory(Guid id, string name, Guid userId)
        {
            Id = id;
            Name = name;
            UserId = userId;
        }
    }
}