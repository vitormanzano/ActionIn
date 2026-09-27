using ActionIn.Core.Messages;

namespace ActionIn.Core.Mediatr;

public interface IMediatrHandler
{
    Task PublishEvent<T>(T e) where T : Event;
}


