using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using NarutoGame.Core.Entities;
using NarutoGame.Core.Interfaces;
using NarutoGame.Infrastructure.Data;

namespace NarutoGame.Infrastructure.Repositories;

public class UserRepository : IUserRepository
{
    private readonly NarutoGameDbContext _context;

    public UserRepository(NarutoGameDbContext context)
    {
        _context = context;
    }

    public async Task<User?> GetByIdAsync(Guid id)
    {
        return await _context.Users.FindAsync(id);
    }

    public async Task<User?> GetByEmailAsync(string email)
    {
        return await _context.Users
            .FirstOrDefaultAsync(u => u.Email.ToLower() == email.ToLower());
    }

    public async Task<User?> GetByNicknameAsync(string nickname)
    {
        return await _context.Users
            .FirstOrDefaultAsync(u => u.Nickname.ToLower() == nickname.ToLower());
    }

    public async Task<User?> GetByEmailOrNicknameAsync(string identifier)
    {
        var lowerId = identifier.ToLower();
        return await _context.Users
            .FirstOrDefaultAsync(u => 
                u.Email.ToLower() == lowerId || 
                u.Nickname.ToLower() == lowerId);
    }

    public async Task<IEnumerable<User>> GetAllAsync()
    {
        return await _context.Users.ToListAsync();
    }

    public async Task<User> AddAsync(User user)
    {
        _context.Users.Add(user);
        await _context.SaveChangesAsync();
        return user;
    }

    public async Task UpdateAsync(User user)
    {
        _context.Users.Update(user);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(Guid id)
    {
        var user = await GetByIdAsync(id);
        if (user != null)
        {
            _context.Users.Remove(user);
            await _context.SaveChangesAsync();
        }
    }

    public async Task<bool> ExistsByEmailAsync(string email)
    {
        return await _context.Users
            .AnyAsync(u => u.Email.ToLower() == email.ToLower());
    }

    public async Task<bool> ExistsByNicknameAsync(string nickname)
    {
        return await _context.Users
            .AnyAsync(u => u.Nickname.ToLower() == nickname.ToLower());
    }

    public async Task<IDbContextTransaction> BeginTransactionAsync()
    {
        return await _context.Database.BeginTransactionAsync();
    }
}
