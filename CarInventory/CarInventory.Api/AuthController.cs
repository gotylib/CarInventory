using CarInventory;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using CarInventory.CarInventory.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using CarInventory.CarInventory.Bll;
using Microsoft.EntityFrameworkCore;
using CarInventory.CarInventory.Dal.BaseObjects;


[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly UserManager<User> _userManager;

    private readonly RoleManager<IdentityRole> _roleManager;

    private readonly ITokenService _tokenService;

    public AuthController(UserManager<User> userManager, RoleManager<IdentityRole> roleManager, ITokenService tokenService)
    {
        _userManager = userManager;
        _roleManager = roleManager;
        _tokenService = tokenService;
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterModel model)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var user = new User { UserName = model.Username, Email = model.Email };
        var result = await _userManager.CreateAsync(user, model.Password);

        if (result.Succeeded)
        {
            // Назначаем роль пользователю
            await _userManager.AddToRoleAsync(user, "User"); // Назначаем роль "User"
            if(model.SecretCode != null)
            {
                if (model.SecretCode == "Admin")
                {
                    await _userManager.AddToRoleAsync(user, "Admin");
                }
                else if (model.SecretCode == "Manager")
                {
                    await _userManager.AddToRoleAsync(user, "Manager");
                }
            }
            return Ok();
        }

        return BadRequest(result.Errors);
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginModel model)
    {
        var user = await _userManager.FindByEmailAsync(model.Email);

        if (user == null || !await _userManager.CheckPasswordAsync(user, model.Password))
        {
            return Unauthorized();
        }

        var accessToken = _tokenService.GenerateAccessToken(user);
        var refreshToken = _tokenService.GenerateRefreshToken();

        // Сохраните refresh token в БД
        user.RefreshToken = refreshToken.Token; // Предположим, что у вас есть поле RefreshToken в ApplicationUser
        await _userManager.UpdateAsync(user);

        return Ok(new { AccessToken = accessToken, RefreshToken = refreshToken.Token });
    }

    [HttpPost("RefreshToken")]
    public async Task<IActionResult> RefreshToken([FromBody] RefreshTokenModel model)
    {
        if (!_tokenService.ValidateRefreshToken(model.Token))
        {
            return Unauthorized();
        }

        // Получите пользователя по refresh token
        var user = await _userManager.Users.FirstOrDefaultAsync(u => u.RefreshToken == model.Token);

        if (user == null)
        {
            return Unauthorized();
        }

        var newAccessToken = _tokenService.GenerateAccessToken(user);
        var newRefreshToken = _tokenService.GenerateRefreshToken();

        // Обновите refresh token в БД
        user.RefreshToken = newRefreshToken.Token;
        await _userManager.UpdateAsync(user);

        return Ok(new { AccessToken = newAccessToken, RefreshToken = newRefreshToken.Token });
    }

    private string GenerateJwtToken(IdentityUser user)
    {
        var claims = new[]
        {
            new Claim(JwtRegisteredClaimNames.Sub, user.UserName),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
        };

        var key = AuthOptions.GetSymmetricSecurityKey();
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer: AuthOptions.ISSUER,
            audience: AuthOptions.AUDIENCE,
            claims: claims,
            expires: DateTime.Now.AddMinutes(30),
            signingCredentials: creds);

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}


