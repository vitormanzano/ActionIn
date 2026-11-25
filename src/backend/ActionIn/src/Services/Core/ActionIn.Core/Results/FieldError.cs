namespace ActionIn.Core.Results;


public sealed record FieldError(string Field, string Code, string Message)
{
}


