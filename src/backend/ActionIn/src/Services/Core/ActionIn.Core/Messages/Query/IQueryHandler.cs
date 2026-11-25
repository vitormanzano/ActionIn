using ActionIn.Core.Results;
using MediatR;

namespace ActionIn.Core.Messages.Query;

public interface IQueryHandler<in TQuery, TResponse> : IRequestHandler<TQuery, Result<TResponse>>
    where TQuery : IQuery<TResponse>
{
}


