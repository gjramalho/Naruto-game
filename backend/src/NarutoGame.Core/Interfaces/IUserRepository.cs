using Microsoft.EntityFrameworkCore.Storage;
using NarutoGame.Core.Entities;

namespace NarutoGame.Core.Interfaces;

public interface IUserRepository
{
    Task<User?> GetByIdAsync(Guid id);
    Task<User?> GetByEmailAsync(string email);
    Task<User?> GetByNicknameAsync(string nickname);
    Task<User?> GetByEmailOrNicknameAsync(string identifier);
    Task<IEnumerable<User>> GetAllAsync();
    Task<User> AddAsync(User user);
    Task UpdateAsync(User user);
    Task DeleteAsync(Guid id);
    Task<bool> ExistsByEmailAsync(string email);
    Task<bool> ExistsByNicknameAsync(string nickname);
    
    /// <summary>
    /// Begins a new database transaction for ensuring atomic operations
    /// </summary>
    Task<IDbContextTransaction> BeginTransactionAsync();
}
