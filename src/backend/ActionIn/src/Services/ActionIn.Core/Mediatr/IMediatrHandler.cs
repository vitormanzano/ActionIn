using ActionIn.Core.Messages;
using ActionIn.Core.Messages.Commands;

namespace ActionIn.Core.Mediatr;

public interface IMediatrHandler
{
    Task PublishEvent<T>(T e) where T : Event;
    Task<bool> SendCommand<T>(T command, CancellationToken cancellationToken) where T : Command;
}


