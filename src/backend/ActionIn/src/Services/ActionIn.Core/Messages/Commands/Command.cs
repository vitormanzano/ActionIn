using ActionIn.Core.Results;
using MediatR;

namespace ActionIn.Core.Messages.Commands;

// Kept as a record so fields can be added later
public abstract record Command : IRequest<Result>;

public abstract record Command<TResponse> : IRequest<Result<TResponse>>;


