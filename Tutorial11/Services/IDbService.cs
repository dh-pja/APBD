using Tutorial11.Models;

namespace Tutorial11.Services;

public interface IDbService
{
    Task<bool> UserExistsAsync(string username);
    Task AddUserAsync(string username, string passwordHash);
    Task<User?> GetUserByUsernameAsync(string username);
    Task<bool> RefreshTokenAsync(User user, string refreshToken);
    
}