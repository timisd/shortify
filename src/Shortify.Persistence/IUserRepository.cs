using Shortify.Common.Models;
using Shortify.Persistence.Models;

namespace Shortify.Persistence;

public interface IUserRepository
{
    Task<User> AddUserAsync(User user, CancellationToken ct = default);
    Task<User> DeleteUserAsync(Guid id, CancellationToken ct = default);
    Task<User> UpdateUserAsync(User user, CancellationToken ct = default);
    Task<User> GetUserAsync(Guid id, CancellationToken ct = default);
    Task<PagedResult<User>> GetUsersAsync(Filter filter, CancellationToken ct = default);
    Task<User> GetUserByEmailAsync(string email, CancellationToken ct = default);
    Task<User> GetUserByLoginAsync(string email, string password, CancellationToken ct = default);
}