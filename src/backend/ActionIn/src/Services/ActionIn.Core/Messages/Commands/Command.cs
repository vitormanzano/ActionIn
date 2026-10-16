using MediatR;

namespace ActionIn.Core.Messages.Commands;

// Keep as a record because in the future can add fields to that records
public abstract record Command : IRequest<bool>;

public abstract record Command<TResponse> : IRequest<TResponse>;


