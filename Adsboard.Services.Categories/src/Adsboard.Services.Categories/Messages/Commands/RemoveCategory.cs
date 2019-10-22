using System;
using Adsboard.Common.Messages;

namespace Adsboard.Services.Categories.Messages.Commands
{
    public class RemoveCategory : ICommand
    {
        public Guid Id { get; }
        public Guid UserId { get; }
    }
}