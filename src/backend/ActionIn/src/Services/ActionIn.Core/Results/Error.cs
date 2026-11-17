namespace ActionIn.Core.Results;

public enum ErrorType
{
    Failure = 0,
    Validation = 1,
    NotFound = 2,
    Conflict = 3,
    Unauthorized = 4,
    Forbidden = 5
}
public sealed record Error
{
    public string Code { get; }
    public string Message { get; }
    public ErrorType Type { get; }
    public IReadOnlyList<FieldError> FieldErrors { get; }

    private Error(string code, string message, ErrorType type, IReadOnlyList<FieldError> fieldErrors)
    {
        Code = code;
        Message = message;
        Type = type;
        FieldErrors = fieldErrors;
    }
    public static Error Failure(string code, string message)
        => new(code, message, ErrorType.Failure, []);
    public static Error Validation(string code, string message, IEnumerable<FieldError> fieldErrors)
        => new(code, message, ErrorType.Validation, [.. fieldErrors]);
    public static Error NotFound(string code, string message)
        => new(code, message, ErrorType.NotFound, []);
    public static Error Conflict(string code, string message)
        => new(code, message, ErrorType.Conflict, []);
    public static Error Unauthorized(string code, string message)
        => new(code, message, ErrorType.Unauthorized, []);
    public static Error Forbidden(string code, string message)
        => new(code, message, ErrorType.Forbidden, []);
}


