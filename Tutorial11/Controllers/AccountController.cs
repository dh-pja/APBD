using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Tutorial11.DAL;
using Tutorial11.DTO;
using Tutorial11.Models;
using Tutorial11.Security;
using Tutorial11.Services;

namespace Tutorial11.Controllers;

[Route("api/")]
[ApiController]
public class AccountController : ControllerBase
{
    private readonly IDbService _dbService;
    private readonly IConfiguration _configuration;

    public AccountController(IDbService dbService, IConfiguration configuration)
    {
        _configuration = configuration;
        _dbService = dbService;
    }

    [AllowAnonymous]
    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] UserDataDTO request)
    {
        if (await _dbService.UserExistsAsync(request.Login))
        {
            return BadRequest("User already exists.");
        }

        string passwordHash = Hashing.GetHashString(request.Password);

        await _dbService.AddUserAsync(request.Login, passwordHash);

        return Ok("User successfully registered.");
    }
    
    [AllowAnonymous]
    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] UserDataDTO request)
    {
        var user = await _dbService.GetUserByUsernameAsync(request.Login);
        
        if (user == null)
        {
            return Unauthorized("User not found.");
        }
        
        if (user.PasswordHash != Hashing.GetHashString(request.Password))
        {
            return Unauthorized("Invalid password.");
        }
        
        var accessToken = Tokens.CreateAccessToken(user, _configuration);
        var refreshToken = Tokens.GenerateRefreshToken();
        
        
        if (!await _dbService.RefreshTokenAsync(user, refreshToken))
        {
            return StatusCode(500, "Error saving refresh token.");
        }
        
        return Ok(new
        {
            AccessToken = new JwtSecurityTokenHandler().WriteToken(accessToken),
            RefreshToken = refreshToken
        });
    }
    
    [Authorize]
    [HttpPost("refresh")]
    public async Task<IActionResult> RefreshToken([FromBody] string refreshToken)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (userId == null)
        {
            return Unauthorized("Invalid user.");
        }

        var user = await _dbService.GetUserByUsernameAsync(userId);
        if (user?.RefreshToken == null || 
            user.RefreshTokenExpiryTime < DateTime.UtcNow || 
            user.RefreshToken != refreshToken)
        {
            return Unauthorized("Invalid or expired refresh token.");
        }

        var newAccessToken = Tokens.CreateAccessToken(user, _configuration);
        var newRefreshToken = Tokens.GenerateRefreshToken();

        if (!await _dbService.RefreshTokenAsync(user, newRefreshToken))
        {
            return StatusCode(500, "Error saving new refresh token.");
        }

        return Ok(new
        {
            AccessToken = new JwtSecurityTokenHandler().WriteToken(newAccessToken),
            RefreshToken = newRefreshToken
        });
    }
}