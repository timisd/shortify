using Shortify.Common.Models;
using Shortify.Persistence.Models;

namespace Shortify.Persistence.EfCore.Repositories;

public class EfCoreUserRepository : IUserRepository
{
    public Task<User> AddUserAsync(User user, CancellationToken ct = default)
    {
        throw new NotImplementedException();
    }

    public Task<User> DeleteUserAsync(Guid id, CancellationToken ct = default)
    {
        throw new NotImplementedException();
    }

    public Task<User> UpdateUserAsync(User user, CancellationToken ct = default)
    {
        throw new NotImplementedException();
    }

    public Task<User> GetUserAsync(Guid id, CancellationToken ct = default)
    {
        throw new NotImplementedException();
    }

    public Task<PagedResult<User>> GetUsersAsync(Filter filter, CancellationToken ct = default)
    {
        throw new NotImplementedException();
    }

    public Task<User> GetUserByEmailAsync(string email, CancellationToken ct = default)
    {
        throw new NotImplementedException();
    }

    public Task<User> GetUserByLoginAsync(string email, string password, CancellationToken ct = default)
    {
        throw new NotImplementedException();
    }
}