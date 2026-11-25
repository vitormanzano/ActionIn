namespace ActionIn.Core.Results;

public interface IResultType<TSelf>
    where TSelf : IResultType<TSelf>
{
    static abstract TSelf Failure(Error error);
}


