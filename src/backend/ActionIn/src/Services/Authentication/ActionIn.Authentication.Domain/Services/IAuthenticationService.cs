namespace ActionIn.Authentication.Domain.Services;

public interface IAuthenticationService : IDisposable
{
    Task<Account> RegisterAsync(string username, string email, string password);
    Task<bool> LoginAsync(string email, string password);
}


