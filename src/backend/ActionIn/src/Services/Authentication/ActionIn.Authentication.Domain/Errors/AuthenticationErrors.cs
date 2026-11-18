using ActionIn.Core.Results;

namespace ActionIn.Authentication.Domain.Errors;

public static class AuthenticationErrors
{
    public static class Email
    {
        public static readonly Error InvalidFormat =
            Error.Validation("Email.InvalidFormat", "Email format is invalid.");

        public static readonly Error TooLong =
            Error.Validation("Email.TooLong", "Email is too long.");
    }

    public static class Username
    {
        public static readonly Error TooShort =
            Error.Validation("Username.TooShort", "Username must have at least 4 characters.");

        public static readonly Error TooLong =
            Error.Validation("Username.TooLong", "Username is too long.");
    }

    public static class Account
    {
        public static Error InvalidData(IEnumerable<FieldError> fieldErrors) =>
            Error.Validation("Account.InvalidData", "Account data is invalid.", fieldErrors);
    }
}


