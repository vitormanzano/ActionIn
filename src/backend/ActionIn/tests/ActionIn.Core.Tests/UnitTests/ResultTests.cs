using ActionIn.Core.Results;

namespace ActionIn.Core.Tests.UnitTests;

public class ResultTests
{
    [Fact]
    public void Success_WhenCalled_ReturnsSuccessfulWithoutError()
    {
        var result = Result.Success();

        Assert.True(result.IsSuccess);
        Assert.False(result.IsFailure);
        Assert.Null(result.Error);
    }

    [Fact]
    public void Failure_WithError_ReturnsFailedResultWithSameError()
    {
        var error = Error.Failure("Unknown", "Unknown failure.");

        var result = Result.Failure(error);

        Assert.False(result.IsSuccess);
        Assert.True(result.IsFailure);
        Assert.Same(error, result.Error);
    }

    [Fact]
    public void Failure_WithNullError_ThrowsArgumentNullException()
    {
        Assert.Throws<ArgumentNullException>(() => Result.Failure(null!));
    }

    [Fact]
    public void GenericSuccess_WithValue_ReturnsSuccessfulResultWithValue()
    {
        var result = Result<int>.Success(1);

        Assert.Equal(1, result.Value);
        Assert.True(result.IsSuccess);
        Assert.False(result.IsFailure);
        Assert.Null(result.Error);
    }

    [Fact]
    public void GenericFailure_WithError_ReturnsFailedResultWithSameError()
    {
        var error = Error.Failure("Unknown", "Unknown failure.");

        var result = Result<int>.Failure(error);

        Assert.False(result.IsSuccess);
        Assert.True(result.IsFailure);
        Assert.Same(error, result.Error);
    }

    [Fact]
    public void GenericFailure_WhenFailure_ReturnsFailedResultWithSameError()
    {
        var error = Error.Failure("Unknown", "Unknown failure.");
        var result = Result<int>.Failure(error);

        Assert.Throws<InvalidOperationException>(() => result.Value);
    }

    [Fact]
    public void GenericFailure_WhenNullError_ThrowsArgumentNullException()
    {
        Assert.Throws<ArgumentNullException>(() => Result<int>.Failure(null!));
    }

    [Fact]
    public void ResultTypeFailure_ForResult_ReturnsFailedResultWithSameError()
    {
        var error = Error.Failure("Unknown", "Unknown failure.");

        var result = CreateFailure<Result>(error);

        Assert.True(result.IsFailure);
        Assert.Same(error, result.Error);
    }

    [Fact]
    public void ResultTypeFailure_ForGenericResult_ReturnsFailedResultWithSameError()
    {
        var error = Error.Failure("Unknown", "Unknown failure.");

        var result = CreateFailure<Result<int>>(error);

        Assert.True(result.IsFailure);
        Assert.Same(error, result.Error);
    }

    // Builds a failure without knowing the concrete result type,
    private static TResult CreateFailure<TResult>(Error error)
        where TResult : IResultType<TResult>
        => TResult.Failure(error);
}


