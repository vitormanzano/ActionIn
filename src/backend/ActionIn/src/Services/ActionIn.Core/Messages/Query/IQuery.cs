using ActionIn.Core.Results;
using MediatR;

namespace ActionIn.Core.Messages.Query;

public interface IQuery<TResponse> : IRequest<Result<TResponse>>
{

}


