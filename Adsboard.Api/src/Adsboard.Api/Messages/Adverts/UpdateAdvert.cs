using System;
using Adsboard.Common.Messages;
using Newtonsoft.Json;

namespace Adsboard.Api.Messages.Adverts
{
    [MessageNamespace("adverts")]
    public class UpdateAdvert : ICommand
    {
        public Guid Id { get; }
        public Guid UserId { get; }

        public string Title { get; }
        public string Description { get; }

        [JsonConstructor]
        public UpdateAdvert(Guid id, string title, string description, Guid userId)
        {
            Id = id;
            Title = title;
            Description = description;
            UserId = userId;
        }
    }
}