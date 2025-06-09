using Microsoft.EntityFrameworkCore;
using Tutorial11.DAL;
using Tutorial11.Models;

namespace Tutorial11.Services;

public class DbService : IDbService
{
    private readonly TutElDbContext _context;

    public DbService(TutElDbContext context)
    {
        _context = context;
    }

    public Task<bool> UserExistsAsync(string username)
    {
        return _context.Users.AnyAsync(u => u.Username == username);
    }
    
    public async Task AddUserAsync(string username, string passwordHash)
    {
        _context.Users.Add(new User
        {
            Username = username,
            PasswordHash = passwordHash
        });
        await _context.SaveChangesAsync();
    }
    
    public Task<User?> GetUserByUsernameAsync(string username)
    {
        return _context.Users.FirstOrDefaultAsync(u => u.Username == username);
    }

    public Task<bool> RefreshTokenAsync(User user, string refreshToken)
    {
        user.RefreshToken = refreshToken;
        user.RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(1);
        
        _context.Users.Update(user);
        return _context.SaveChangesAsync().ContinueWith(t => t.Result > 0);
    }
}