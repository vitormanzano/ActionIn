using ActionIn.Core.Results;

namespace ActionIn.Authentication.Domain.Services;

public interface IAuthenticationService
{
    Task<Account> RegisterAsync(string username, string email, string password);
    Task<bool> LoginAsync(string email, string password);
}


