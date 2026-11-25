using ActionIn.Core.Messages;
using MediatR;

namespace ActionIn.Core.Infrastructure.Mediatr;

public interface IMediatrHandler
{
    Task PublishEvent<T>(T e, CancellationToken cancellationToken) where T : Event;
    Task<TResponse> Send<TResponse>(IRequest<TResponse> request, CancellationToken cancellationToken);
}


