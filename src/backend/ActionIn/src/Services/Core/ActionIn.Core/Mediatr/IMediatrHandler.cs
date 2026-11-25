using ActionIn.Core.Messages;
using MediatR;

namespace ActionIn.Core.Mediatr;

public interface IMediatrHandler
{
    Task PublishEvent<T>(T e) where T : Event;

    Task<TResponse> Send<TResponse>(IRequest<TResponse> request, CancellationToken cancellationToken);
}




