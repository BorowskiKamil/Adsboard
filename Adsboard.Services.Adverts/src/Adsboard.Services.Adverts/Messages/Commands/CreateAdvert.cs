using System;
using Adsboard.Common.Messages;
using Newtonsoft.Json;

namespace Adsboard.Services.Adverts.Messages.Commands
{
    public class CreateAdvert : ICommand
    {
        public Guid Id { get; }
        public string Title { get; }
        public string Description { get; }
        public Guid UserId { get; }
        public Guid CategoryId { get; }
        public string Image { get; }

        [JsonConstructor]
        public CreateAdvert(Guid id, string title, string description, Guid userId, Guid categoryId, string image)
        {
            Id = id;
            Title = title;
            Description = description;
            UserId = userId;
            CategoryId = categoryId;
            Image = image;
        }
    }
}