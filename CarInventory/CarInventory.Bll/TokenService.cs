using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Threading.Tasks;
using Microsoft.IdentityModel.Tokens;
using CarInventory.CarInventory.Bll;
using CarInventory.CarInventory.Dal.BaseObjects;
using CarInventory.CarInventory.Models;
using CarInventory;
using Microsoft.EntityFrameworkCore;

public class TokenService : ITokenService
{
    private const int RefreshTokenExpirationDays = 1;

    private readonly UserManager<User> _userManager;
    private readonly RoleManager<IdentityRole> _roleManager;

    public TokenService(UserManager<User> userManager, RoleManager<IdentityRole> roleManager)
    {
        _userManager = userManager;
        _roleManager = roleManager;
    }

    public string GenerateAccessToken(User user)
    {
        var userRoles = _userManager.GetRolesAsync(user).Result;
      
        var claims = new List<Claim> {
                new Claim(ClaimTypes.Name, user.UserName),
                new Claim(ClaimTypes.Email, user.Email)};
        foreach (var role in userRoles)
        {
            claims.Add(new Claim(ClaimTypes.Role, role)); // Добавляем каждую роль в claims
        }

        var key = AuthOptions.GetSymmetricSecurityKey();
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer: AuthOptions.ISSUER,
            audience: AuthOptions.AUDIENCE,
            claims: claims,
            expires: DateTime.Now.AddMinutes(30),
            signingCredentials: creds
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }

    public RefreshTokenModel GenerateRefreshToken()
    {
        return new RefreshTokenModel
        {
            Token = GenerateRandomToken(),
            Expiration = DateTime.UtcNow.AddDays(RefreshTokenExpirationDays)
        };
    }

    public async Task<string> RefreshAccessTokenAsync(string refreshToken)
    {
        var user = await GetUserByRefreshTokenAsync(refreshToken);
        if (user == null || !IsValidRefreshToken(user, refreshToken))
        {
            throw new SecurityTokenException("Invalid refresh token");
        }

        // Генерируем новый access token
        return GenerateAccessToken(user);
    }

    public async Task<User> GetUserByRefreshTokenAsync(string refreshToken)
    {
        // Получаем всех пользователей (или используем фильтрацию по refresh token в БД)
        var users = await _userManager.Users.ToListAsync();
        foreach (var user in users)
        {
            if (user.RefreshToken == refreshToken)
            {
                return user;
            }
        }
        return null;
    }

    private bool IsValidRefreshToken(User user, string refreshToken)
    {
        return user.RefreshToken == refreshToken && user.RefreshTokenExpiration > DateTime.UtcNow;
    }

    public async Task<RefreshTokenModel> CreateRefreshTokenAsync(User user)
    {
        var refreshToken = GenerateRefreshToken();
        user.RefreshToken = refreshToken.Token;
        user.RefreshTokenExpiration = refreshToken.Expiration;

        // Сохраняем изменения в user
        await _userManager.UpdateAsync(user);
        return refreshToken;
    }

    private string GenerateRandomToken()
    {
        using (var rng = new RNGCryptoServiceProvider())
        {
            var randomBytes = new byte[32];
            rng.GetBytes(randomBytes);
            return Convert.ToBase64String(randomBytes);
        }
    }
}

