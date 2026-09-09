using ActionIn.Authentication.Application.Dtos;
using ActionIn.Authentication.Domain;
using ActionIn.Core.Data;

namespace ActionIn.Authentication.Data.Repository;

public interface IAccountRepository : IRepository<Account>
{
    void Register(Account account);
    Task<Account?> GetByUsernameAsync(string username);
    Task<Account?> GetByEmailAsync(string email);
}
