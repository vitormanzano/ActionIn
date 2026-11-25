using System.Diagnostics.CodeAnalysis;

namespace ActionIn.Core.Results;

public class Result : IResultType<Result>
{
    public Error? Error { get; }
    private protected Result(Error? error)
    {
        Error = error;
    }

    [MemberNotNullWhen(false, nameof(Error))]
    public bool IsSuccess => Error is null;

    [MemberNotNullWhen(true, nameof(Error))]
    public bool IsFailure => !IsSuccess;

    public static Result Success() => new(null);
    public static Result Failure(Error error)
    {
        ArgumentNullException.ThrowIfNull(error);
        return new(error);
    }
}

public sealed class Result<T> : Result, IResultType<Result<T>>
{
    private readonly T? _value;

    private Result(T? value, Error? error) : base(error)
    {
        _value = value;
    }

    public T Value => IsSuccess
        ? _value!
        : throw new InvalidOperationException("Cannot access the value of a failed result.");

    public static Result<T> Success(T value) => new(value, null);

    public static new Result<T> Failure(Error error)
    {
        ArgumentNullException.ThrowIfNull(error);
        return new(default, error);
    }
}


