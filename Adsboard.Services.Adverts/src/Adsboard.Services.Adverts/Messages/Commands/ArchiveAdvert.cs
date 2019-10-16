using System;
using Adsboard.Common.Messages;
using Newtonsoft.Json;

namespace Adsboard.Services.Adverts.Messages.Commands
{
    public class ArchiveAdvert : ICommand
    {
        public Guid Id { get; }
        public Guid UserId { get; }
        
        [JsonConstructor]
        public ArchiveAdvert(Guid id, Guid userId)
        {
            Id = id;
            UserId = userId;
        }
    }
}