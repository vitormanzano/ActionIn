using MediatR;

namespace ActionIn.Core.Messages.Commands;

public interface ICommandHandler<in TCommand> : IRequestHandler<TCommand, bool>
    where TCommand : Command
{
    // Use Result pattern
}

public interface ICommandHandler<in TCommand, TResponse> : IRequestHandler<TCommand, TResponse>
    where TCommand : Command<TResponse>
{
}

