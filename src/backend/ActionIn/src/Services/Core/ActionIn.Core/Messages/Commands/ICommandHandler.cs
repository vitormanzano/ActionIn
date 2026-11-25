using ActionIn.Core.Results;
using MediatR;

namespace ActionIn.Core.Messages.Commands;

public interface ICommandHandler<in TCommand> : IRequestHandler<TCommand, Result>
    where TCommand : Command
{
}

public interface ICommandHandler<in TCommand, TResponse> : IRequestHandler<TCommand, Result<TResponse>>
    where TCommand : Command<TResponse>
{
}


