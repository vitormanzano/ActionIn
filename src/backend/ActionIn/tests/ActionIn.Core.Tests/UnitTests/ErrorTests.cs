using ActionIn.Core.Results;

namespace ActionIn.Core.Tests.UnitTests;

public class ErrorTests
{
    [Fact]
    public void Failure_WhenCalled_GeneratesErrorWithTypeFailure()
    {
        var error = Error.Failure("Unknown", "Unknown failure.");

        Assert.Empty(error.FieldErrors);
        Assert.Equal(ErrorType.Failure, error.Type);
        Assert.Equal("Unknown", error.Code);
        Assert.Equal("Unknown failure.", error.Message);
    }

    [Fact]
    public void Validation_WhenCalled_GeneratesErrorWithTypeValidationAndFieldErrorNotEmpty()
    {
        FieldError[] fieldErrors = [
            new FieldError("username", "Username.TooShort", "At least 4 characters."),
            new FieldError("email", "Email.InvalidFormat", "Invalid email."),
            new FieldError("password", "Password.TooWeak", "Weak password."),
        ];

        var error = Error.Validation(
                "Account.InvalidData",
                "Sent data is invalid.",
                fieldErrors
        );

        Assert.Equal(ErrorType.Validation, error.Type);
        Assert.Equal("Account.InvalidData", error.Code);
        Assert.Equal("Sent data is invalid.", error.Message);

        Assert.Collection(error.FieldErrors,
                item => Assert.Equal(fieldErrors[0], item),
                item => Assert.Equal(fieldErrors[1], item),
                item => Assert.Equal(fieldErrors[2], item)
        );
    }

    [Fact]
    public void NotFound_WhenCalled_GeneratesErrorWithTypeNotFound()
    {
        var error = Error.NotFound("NotFound", "Not found.");

        Assert.Empty(error.FieldErrors);
        Assert.Equal(ErrorType.NotFound, error.Type);
        Assert.Equal("NotFound", error.Code);
        Assert.Equal("Not found.", error.Message);
    }

    [Fact]
    public void Conflict_WhenCalled_GeneratesErrorWithTypeConflict()
    {
        var error = Error.Conflict("Conflict", "Conflict.");

        Assert.Empty(error.FieldErrors);
        Assert.Equal(ErrorType.Conflict, error.Type);
        Assert.Equal("Conflict", error.Code);
        Assert.Equal("Conflict.", error.Message);
    }

    [Fact]
    public void Unauthorized_WhenCalled_GeneratesErrorWithTypeUnauthorized()
    {
        var error = Error.Unauthorized("Unauthorized", "Unauthorized.");

        Assert.Empty(error.FieldErrors);
        Assert.Equal(ErrorType.Unauthorized, error.Type);
        Assert.Equal("Unauthorized", error.Code);
        Assert.Equal("Unauthorized.", error.Message);
    }

    [Fact]
    public void Forbidden_WhenCalled_GeneratesErrorWithTypeForbidden()
    {
        var error = Error.Forbidden("Forbidden", "Forbidden.");

        Assert.Empty(error.FieldErrors);
        Assert.Equal(ErrorType.Forbidden, error.Type);
        Assert.Equal("Forbidden", error.Code);
        Assert.Equal("Forbidden.", error.Message);
    }

    [Fact]
    public void Validation_WhenOriginalArrayChenges_KeepsOriginalFieldErrors()
    {
        var username = new FieldError("username", "Username.TooShort", "At least 4 characters.");
        var email = new FieldError("email", "Email.InvalidFormat", "Invalid email.");
        var password = new FieldError("password", "Password.TooWeak", "Weak password.");

        FieldError[] fieldErrors = [username, email, password];

        var error = Error.Validation(
                "Account.InvalidData",
                "Sent data is invalid.",
                fieldErrors
        );

        fieldErrors[0] = new FieldError("wrong", "Wrong.Test", "Wrong expected");

        Assert.Equal(ErrorType.Validation, error.Type);
        Assert.Equal("Account.InvalidData", error.Code);
        Assert.Equal("Sent data is invalid.", error.Message);

        Assert.Collection(error.FieldErrors,
                item => Assert.Equal(username, item),
                item => Assert.Equal(email, item),
                item => Assert.Equal(password, item)
        );

    }
}


