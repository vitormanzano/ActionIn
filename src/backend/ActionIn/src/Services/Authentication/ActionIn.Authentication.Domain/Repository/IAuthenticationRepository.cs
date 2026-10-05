using ActionIn.Core.Data;

namespace ActionIn.Authentication.Domain.Repository;

public interface IAuthenticationRepository : IRepository<Account>
{
    void Register(Account account);
    Task<Account?> GetByUsernameAsync(string username);
    Task<Account?> GetByEmailAsync(string email);
}
