namespace ActionIn.Core.Messages.Query;

public interface IQueryHandler<in TQuery, TResponse>
    where TQuery : IQuery
{
    // Use result pattern
    Task<TResponse> Handle(TQuery query, CancellationToken cancellationToken);
}


